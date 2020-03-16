# The New .NET for Apache® Spark™ Take on DataFrame

[.NET for Apache Spark][1] is aimed at making [Apache® Spark™][2], and thus the exciting world of big data analytics, accessible to .NET developers. .NET for Spark can be used for processing batches of data, real-time streams, machine learning, and ad-hoc query.

The [DataFrame][3] is one of the core data structures in Spark programming. A DataFrame is a distributed collection of data organized into named columns. In a Spark application, we typically start off by reading input data from a data source, storing it in a DataFrame, and then leveraging functionality like Spark SQL to manipulate, display, and gain insights from our data. User-defined functions, or UDFs, are column-based functions that allow us to transform and gain new insights from data stored in DataFrames.

In December 2019, the [.NET team announced][4] the preview of the [Microsoft.Data.Analysis][5] DataFrame type to make data exploration easy in .NET. Now in March 2020, we have [introduced convenience APIs][6] to the .NET for Spark codebase for using Microsoft.Data.Analysis DataFrames with UDFs in Spark. These convenience APIs make data manipulation and analysis with UDFs much more convenient and concise in .NET for Spark.

In this blog post, we’ll explore:
*   [Implementation goals and details][7]
*   [Coding examples and DataFrame comparisons][8]
*   [Wrap Up][9]

### <a id="implement"></a>Implementation goals and details

#### Goals

The main goal of this work is to make the process of creating UDFs in .NET for Spark easier and more concise through a set of convenience APIs. 

Let's start off with some context about data-sharing in Spark UDFs. Apache Spark streams data to most modern UDFs in the [Apache Arrow][10] format. Apache Arrow provides a standardized, language-dependent format for working with data in-memory. It's designed for high-performance, efficient analysis through its columnar memory format, and it provides libraries and zero-copy messaging for communication across processes.

Because Spark streams data to UDFs in the Arrow format, a user writing a UDF would need to understand the Arrow format, such as how to read Arrow columns, write to Arrow columns, and unwrap an Arrow RecordBatch (which is a batch of rows of columns of equal length) to manipulate the data it holds. Using Arrow can involved lengthy code and require additional learning so that developers understand how to integrate it correctly in their applications. 

Our integration of Microsoft.Data.Analysis DataFrames provides support for Arrow-formatted data out-of-the-box. Now developers no longer need to work with the Apache Arrow format directly and can instead stick with the data formats they already understand.

#### Details

Prior to this work, users often needed to enumerate an Arrow RecordBatch to work with columns in a UDF. RecordBatch creation and usage can be quite lengthy and disrupt the flow of a user's app since a RecordBatch is an Arrow-based object, not a standard Spark object. But now, our convenience APIs automatically wrap data that would've been stored in a RecordBatch in a Microsoft.Data.Analysis DataFrame instead. Our wrapping doesn’t involve copying data, ensuring performance remains high as our ease of coding also improves. We can use the newly introduced convenience APIs to start reading, writing, and manipulating our data all through DataFrames rather than RecordBatches!

In the goals section above, we mentioned that Spark streams data to most UDFs in the Arrow format. There are a few kinds of Spark UDFs: pickling, scalar, and vector, and our convenience APIs specifically apply to the latter two.

Pickling UDFs are an older, traditional Spark UDF. They leverage pickle serialization to convert data between a JVM object to a Spark-usable pickle object. Once a column is specified for processing, a pickling UDF will take each of its rows, apply the given functionality, and then add a new column, resulting in quite a bit of overhead.

By contrast, scalar and vector UDFs leverage Arrow serialization rather than pickling. By using Arrow, these UDFs can reap the benefits of an in-memory columnar format for data analysis, and the data transfer is more efficient. 

Our new .NET for Apache Spark convenience APIs specifically apply to scalar and vector UDFs since they leverage the Arrow format.

You’ll likely use both traditional Spark DataFrames and the new convenience APIs for Microsoft.Data.Analysis DataFrames in your programs. The traditional DataFrame will be used for the entire dataset in the parts of the Spark driver program outside of UDFs. When you create a UDF, the data in the traditional DataFrame will be streamed to the UDFs in the worker machines in the Arrow format. Once inside the UDF, you’ll now work with the Microsoft.Data.Analysis DataFrame (rather than RecordBatches).

There is currently 1 scenario in which you will not use these convenience APIs and will need to stick with the traditional DataFrames: intrinsics. As shown in [VectorFunctions.intrinsics.cs][11], we can use hardware intrinsics in Spark DataFrames to further boost performance. However, the Microsoft.Data.Analysis DataFrame does not yet support intrinsics. For all other cases, you can choose to use the traditional Arrow-based APIs or the Microsoft.Data.Analysis DataFrame, and the latter will typically be the optimal choice for UDFs with more concise, user-friendly code.  

### <a id="example"></a>Example

#### Simple Examples

As a gentle introduction, let’s start with a common example. Let’s say we have a UDF that adds 2 columns and returns the result. Traditionally, we’d be required to work with a RecordBatch. An Arrow RecordBatch is immutable, so adding 2 columns using the traditional Spark DataFrames would require allocating a new column in the Arrow format and writing for loops to perform the computation. 

If we instead used the Microsoft.Data.Analysis DataFrame, we can write something along the lines of: <pre class="prettyprint">dataframe.ColumnA + dataframe.ColumnB</pre>

Data reading and conversion steps will be taken care of internally by the API behind-the-scenes, thus shortening and improving the readability of your code.

As another example, we often use UDFs that need to return a set of columns. With the traditional Spark DataFrames, these columns must be returned as an Arrow RecordBatch, which can involve lengthy or somewhat challenging code. With our new convenience APIs, we can just return a DataFrame, and everything else is handled internally!

#### Detailed Examples

Let’s take a look at a more detailed, concrete example. [VectorUdfs.cs][12] is a program using traditional Spark vector UDFs. It reads in a Json file with people’s names and ages as input, stores the data in a DataFrame, and then uses a UDF to count the number of characters in each name, finally returning a set of ages + the number of characters in each respective age’s name. 

[VectorDataFrameUdfs.cs][13] is an updated program that accomplishes the same task with Microsoft.Data.Analysis DataFrames. 

Both programs use a Grouped Map Vector UDF and apply it very similarly. In VectorUdfs.cs, the code is as follows:

<pre class="prettyprint">df.GroupBy("age")
    .Apply(
    new StructType(new[]
    {
        new StructField("age", new IntegerType()),
        new StructField("nameCharCount", new IntegerType())
    }),
r => CountCharacters(r, "age", "name"))
</pre>

Both programs implement a `CountCharacters` method to determine the length of the names. By contrast to the snippet above, VectorDataFrameUdfs.cs concludes the UDF by calling CountCharacters with the code `r => CountCharacters(r))`, thus demonstrating the convenience APIs already make data processing in UDFs more concise. 

`CountCharacters` is also implemented differently in each program. In VectorUdfs.cs, the definition is:

<pre class="prettyprint">private static RecordBatch CountCharacters(
            RecordBatch records,
            string groupFieldName,
            string stringFieldName)
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

<pre class="prettyprint">private static FxDataFrame CountCharacters(
            FxDataFrame dataFrame)
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

The traditional VectorUdfs.cs sample requires us to convert to and from the RecordBatch type. We have to spend many lines of code to just be able to read our data and to write it in a format our UDF can understand. 

By contrast, the new VectorDataFrameUdfs.cs sample is able to dive almost immediately into getting the length of our input data, appending it appropriately to our output, and then returning an actual DataFrame as a result. We can still harness the tremendous benefits of Apache Arrow without the extra code overhead or confusion – awesome! 

### <a id="wrapup"></a>Wrap Up

Thank you to Prashanth Govindarajan, Eric Erhardt, Terry Kim, and the other members of the .NET and .NET for Apache Spark teams for their contributions to this outstanding work.

We’d love to help you get started with .NET for Apache Spark and hear your feedback. You can [Request a Demo][1] from our landing page and check out the [].NET for Spark GitHub repo][14] to get involved with our effort to make .NET a great tech stack for building big data applications.

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
