# The New .NET for Apache® Spark™ Take on DataFrame

[.NET for Apache Spark][1] is aimed at making [Apache® Spark™][2], and thus the exciting world of big data analytics, accessible to .NET developers. .NET for Spark can be used for processing batches of data, real-time streams, machine learning, and ad-hoc query.

The [DataFrame][3] is one of the core data structures in Spark programming. A DataFrame is a distributed collection of data organized into named columns. In a Spark application, we typically start off by reading input data from a data source, storing it in a DataFrame, and then leveraging functionality like Spark SQL to manipulate, display, and gain insights from our data. User-defined functions, or UDFs, are column-based functions that allow us to transform and gain new insights from data stored in DataFrames.

In December 2019, the [.NET team announced][4] the preview of the [Microsoft.Data.Analysis][5] DataFrame type to make data exploration easy in .NET. Now in March 2020, we have [introduced convenience APIs][6] to the .NET for Spark codebase for using Microsoft.Data.Analysis DataFrames with UDFs in Spark. These convenience APIs make data manipulation and analysis with UDFs much more convenient and concise in .NET for Spark.

In this blog post, we’ll explore:
*   [Implementation goal and details][7]
*   [Coding examples and DataFrame comparisons][8]
*   [Wrap Up][9]

### <a id="implement"></a>Implementation goals and details

#### Context and Goals

Let's start off with some context about data-sharing in Spark UDFs. Apache Spark streams data to scalar and vector UDFs in the [Apache Arrow][10] format. Apache Arrow provides a standardized, language-independent format for working with data in-memory. It's designed for high-performance, efficient analysis through its columnar memory format, and it provides libraries and zero-copy messaging for communication across processes.

![Diagram of the benefits of Apache Arrow][logo]
> Diagram credit: https://arrow.apache.org/.

Because Spark streams data to UDFs in the Arrow format, you need to understand the Arrow format when working with UDFs, such as how to read Arrow columns, write to Arrow columns, and unwrap a RecordBatch, which is a 2D data type in Arrow consisting of a set of rows of equal-length columns. 

The main goal of the work described in this blog post is to improve scalar and vector UDFs in .NET for Spark through a set of convenience APIs introducing the Microsoft.Data.Analysis DataFrame. Using Microsoft.Data.Analysis DataFrames provides support for Arrow-formatted data out-of-the-box- you can now avoid working with the Apache Arrow format directly as that's taken care of behind-the-scenes.

#### Details

Prior to this work, you needed to enumerate an Arrow RecordBatch to work with columns in a UDF. RecordBatches are Arrow-based objects, not standard Spark objects, and can thus disrupt the flow or familiarity of code in your program. But now, our convenience APIs automatically wrap data that would've been stored in a RecordBatch in a Microsoft.Data.Analysis DataFrame. The wrapping doesn’t involve copying data, thus ensuring performance remains high as our ease of coding also improves. We can use the APIs to start reading, writing, and manipulating our data all through DataFrames rather than RecordBatches.

In the goals section above, we mentioned that Spark streams data to *most* UDFs in the Arrow format. There are a few kinds of Spark UDFs: pickling, scalar, and vector, and our convenience APIs specifically apply to the latter two.

Pickling UDFs are an older version of Spark UDFs. They leverage pickle serialization to convert data between the JVM and .NET for Spark processes. Once a column is specified for processing, a pickling UDF will take each of its rows, apply the given functionality, and then add a new column, resulting in quite a bit of overhead.

By contrast, scalar and vector UDFs leverage Arrow serialization rather than pickling. By using Arrow, these UDFs can reap the benefits of an in-memory columnar format for data analysis, and the data transfer to and from the JVM is more efficient. Our new .NET for Apache Spark convenience APIs specifically apply to scalar and vector UDFs since they leverage the Arrow format.

You can use both kinds of DataFrames in your programs. The traditional Spark SQL DataFrame distributes data across your Spark cluster. It will be used for the entire dataset in the Spark driver program (outside of UDFs). Once you create a UDF, the data in the traditional DataFrame will be streamed to the UDFs in the worker machines in the Arrow format. Once inside the UDF, you’ll now work with the Microsoft.Data.Analysis DataFrame (rather than RecordBatches), which are in-memory on a single machine. The concept of the Microsoft.Data.Analysis DataFrame is similar to the [Python Pandas DataFrame][15].

There is currently 1 scenario in which you will not use these convenience APIs and will need to stick with the traditional DataFrames: intrinsics. As shown in [VectorFunctions.intrinsics.cs][11], we can use hardware intrinsics in Spark to further boost performance. However, the Microsoft.Data.Analysis DataFrame does not yet support intrinsics. 

### <a id="example"></a>Example

#### Simple Examples

Let’s start with a basic example. Suppose we have a vector UDF that adds 2 columns and returns the result. Traditionally, we’d have to use an Arrow RecordBatch. A RecordBatch is immutable, so adding 2 columns would require allocating a new column in the Arrow format and writing for loops to perform the computation. 

If we instead used the Microsoft.Data.Analysis DataFrame, we can write something along the lines of: `dataframe.ColumnA + dataframe.ColumnB`. Isn't that convenient!

As another example, we often create UDFs that return a set of columns. With the traditional Spark DataFrames, these columns must be returned as an Arrow RecordBatch. But with our new convenience APIs, we can just return a DataFrame, and everything else is handled internally!

#### Detailed Examples

Let’s take a look at a more detailed, concrete example. [VectorUdfs.cs][12] is a program using the traditional Spark DataFrame. It reads in a Json file with people’s names and ages as input, stores the data in a DataFrame, and then uses a vector UDF to count the number of characters in each name, finally returning a set of ages + the number of characters in each respective age’s name. 

[VectorDataFrameUdfs.cs][13] is an updated program that accomplishes the same task with the Microsoft.Data.Analysis DataFrame. 

Both programs use a Grouped Map Vector UDF and apply it very similarly. In VectorUdfs.cs, the code is as follows:

<pre class="prettyprint">
df.GroupBy("age")
    .Apply(
    new StructType(new[]
    {
        new StructField("age", new IntegerType()),
        new StructField("nameCharCount", new IntegerType())
    }),
r => CountCharacters(r, "age", "name"))
</pre>

By contrast, VectorDataFrameUdfs.cs concludes the UDF by calling CountCharacters with the code `r => CountCharacters(r))`. Both programs implement a `CountCharacters` method to determine the length of the names, but VectorDataFrameUdfs.cs is already able to make that method call more concise by only requiring 1 input parameter.

`CountCharacters` is implemented differently in each program. In VectorUdfs.cs, the definition is:

<pre class="prettyprint">
private static RecordBatch CountCharacters(RecordBatch records, string groupFieldName, string stringFieldName)
{
    int stringFieldIndex = records.Schema.GetFieldIndex(stringFieldName);
    StringArray stringValues = records.Column(stringFieldIndex) as StringArray;

    int characterCount = 0;

    for (int i = 0; i < stringValues.Length; ++i)
    {
        string current = stringValues.GetString(i);
        characterCount += current.Length;
    }

    int groupFieldIndex = records.Schema.GetFieldIndex(groupFieldName);
    Field groupField = records.Schema.GetFieldByIndex(groupFieldIndex);

    // Return 1 record, if we were given any. 0, otherwise.
    int returnLength = records.Length > 0 ? 1 : 0;

    return new RecordBatch(
        new Schema.Builder()
            .Field(groupField)
            .Field(f => f.Name(stringFieldName + "_CharCount").DataType(Int32Type.Default))
            .Build(),
        new IArrowArray[]
        {
            records.Column(groupFieldIndex),
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

    var characterCountColumn = new PrimitiveDataFrameColumn<int>("name" + "CharCount");
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

Note that the `FxDataFrame` type represents the Microsoft.Data.Analysis DataFrame, while `DataFrame` represents the traditional Spark DataFrame. This is signified in the latter sample at the top of the program:

<pre class="prettyprint">
using DataFrame = Microsoft.Spark.Sql.DataFrame;
using FxDataFrame = Microsoft.Data.Analysis.DataFrame;
</pre>

As you can see, the latter `CountCharacters` implementation deals completely with DataFrames rather than RecordBatches. It is also half the length of the first implementation!

In this single comparison, we can see where these APIs add a great deal of convenience to our .NET for Spark apps. VectorUdfs.cs requires us to convert to and from the RecordBatch type. We have to spend many lines of code to just be able to read our data and to write it in a format our UDF can understand. By contrast, the new VectorDataFrameUdfs.cs sample is able to dive almost immediately into getting the length of our input data, appending it appropriately to our output, and then returning an actual DataFrame as a result. We can still harness the tremendous benefits of Apache Arrow without the extra code overhead or confusion – awesome! 

### <a id="wrapup"></a>Wrap Up

Thank you to Prashanth Govindarajan, Eric Erhardt, Terry Kim, and the other members of the .NET and .NET for Apache Spark teams for their contributions to this outstanding work.

We’d love to help you get started with .NET for Apache Spark and hear your feedback. You can [Request a Demo][1] from our landing page and check out the [.NET for Spark GitHub repo][14] to get involved with our effort to make .NET a great tech stack for building big data applications.

[1]: https://dot.net/spark
[2]: https://spark.apache.org/
[3]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.spark.sql.dataframe?view=spark-dotnet
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
