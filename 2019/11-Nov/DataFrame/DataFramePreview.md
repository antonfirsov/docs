Last month, we announced .NET support for Jupyter notebooks, and showed how to use them to work with .NET for Apache Spark and ML.NET. Today, we're announcing the preview of a [DataFrame](https://www.nuget.org/packages/Microsoft.Data.Analysis/) type for .NET to make data exploration easy. If you've used Python to manipulate data in notebooks, you'll already be familiar with the concept of a DataFrame. At a high level, it is an in-memory representation of structured data. First, let's fire up a .NET Jupyter Notebook in our [browser](https://mybinder.org/v2/gh/dotnet/try/master?urlpath=lab).

## How to use DataFrame?

`DataFrame` stores data as a collection of columns. Let's populate a `DataFrame` with some sample data and go over the major features. The full sample can be found on Github([C#](https://github.com/dotnet/try/tree/master/NotebookExamples/csharp/Samples) and [F#](https://github.com/dotnet/try/tree/master/NotebookExamples/csharp/Samples)). To get started, let's import the [Microsoft.Data.Analysis](https://www.nuget.org/packages/Microsoft.Data.Analysis/) package and namespace into our .NET Jupyter Notebook(make sure you're using a .NET (C# or F#) kernel):

![](Microsoft.Data.Analysis.PNG)

Let's make 3 columns to hold values of types `DateTime`, `int` and `string`. 
``` csharp
PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("DateTimes"); // Default length is 0.
PrimitiveDataFrameColumn<int> ints = new PrimitiveDataFrameColumn<int>("Ints", 3); // Makes a column of length 3. Filled with nulls initially
StringDataFrameColumn strings = new StringDataFrameColumn("Strings", 3); // Makes a column of length 3. Filled with nulls initially
```

`PrimitiveDataFrameColumn` is a generic type that can hold primitive types such as `int`, `float`, `decimal` etc. A `StringDataFrameColumn` is a specialized column that holds `string` values. Both the column types can take a `length` parameter in their contructors to indicate their initial capacity. The constructors fill the columns with `null` values initially. Before we can add these columns to a `DataFrame` though, we need to append 3 values to our `dateTimes` column. This is because the `DataFrame` constructor expects all its columns to have the same length. 

``` csharp
// Append 3 values to dateTimes
dateTimes.Append(DateTime.Parse("2019/01/01"));
dateTimes.Append(DateTime.Parse("2019/01/01"));
dateTimes.Append(DateTime.Parse("2019/01/02"));
```

Now we're ready to create a `DataFrame` with 3 columns.

``` csharp
DataFrame df = new DataFrame(new List<DataFrameColumn> { dateTimes, ints, strings }); // This will throw if the columns are of different lengths
```

One of the benefits of using a notebook for data exploration is the interactive REPL. We can enter `df` into a new cell to see what data it contains.

![Array Print](ArrayPrint.PNG)

We can immediately see that the formatting of the output can be improved. Each column is printed as an array of values and we don't see the names of the columns. If `df` had more rows and columns, the output would be hard to read. Fortunately, in a Jupyter environment, we can write a custom formatter for our `DataFrame`. 

```csharp
using Microsoft.AspNetCore.Html;
Formatter<DataFrame>.Register((df, writer) =>
{
    var headers = new List<IHtmlContent>();
    headers.Add(th(i("index")));
    headers.AddRange(df.Columns.Select(c => (IHtmlContent) th(c.Name)));
    var rows = new List<List<IHtmlContent>>();
    var take = 20;
    for (var i = 0; i < Math.Min(take, df.RowCount); i++)
    {
        var cells = new List<IHtmlContent>();
        cells.Add(td(i));
        foreach (var obj in df[i])
        {
            cells.Add(td(obj));
        }
        rows.Add(cells);
    }
    
    var t = table(
        thead(
            headers),
        tbody(
            rows.Select(
                r => tr(r))));
    
    writer.Write(t);
}, "text/html");
```

This snippet of code register a new `DataFrame` formatter. All subsequent evaluations of `df` in a notebook will now output the first 20 rows of a `DataFrame` along with the column names.

![PrintDataFrame](PrintDataFrame.PNG)

Sure enough, when we re-evaluate `df`, we see that it contains the 3 columns we created previously. The formatting makes it much easier to inspect our values. There's also a helpful `index` column in the output to quickly see which row we're looking at. Let's modify our data by indexing into `df`:

``` csharp
df[0, 1] = 10; // 0 is the rowIndex, and 1 is the columnIndex. This sets the 0th value in the Ints columns to 10
```
![DataFrameIndexing](DataFrameIndexing.PNG)

We can also modify the values in the columns through indexers defined on `PrimitiveDataFrameColumn` and `StringColumn`:

``` csharp
// Modify ints and strings columns by indexing
ints[1] = 100;
strings[1] = "Foo!";
```

![ColumnIndexers](ColumnIndexers.PNG)

One caveat to keep in mind here is the data type of the value passed in to the indexers. We passed in the right data types to the column indexers in our sample: an integer value of `100` to `ints[1]` and a string `"Foo!"` to `string[1]`. If the data types don't match, an exception will be thrown. For cases where the type of data in the columns is not obvious, there is a handy `DataType` property defined on each column. 

![DataType](DataType.PNG)

The `DataFrame` and `DataFrameColumn` classes expose a number of useful APIs: binary operations, computations, joins, merges, handling missing values and more. Let's look at some of them:
``` csharp
// Add 5 to ints through the DataFrame
df["Ints"].Add(5, inPlace: true);
```
``` csharp
// We can also use binary operators. Binary operators produce a copy, so assign it back to our Ints column 
df["Ints"] = (ints / 5) * 100;
```
![Binary Operations](BinaryOperations.PNG)

All binary operators are backed by functions that produces a copy by default. The `+` operator, for example, calls the `Add` method and passes in `false` for the `inPlace` parameter. This lets us elegantly perform data manipulation using operators without worrying about modifying our existing values. For when in place semantics are desired, we can set the `inPlace` parameter to `true` in the binary functions.  

Often, we read in data from an existing dataset that may contain `null` values. `DataFrame` has the `LoadCsv` method to read in csv files. 
``` csharp
DataFrame csvDataFrame = DataFrame.LoadCsv("path/to/file.csv");
```

In our sample, `df` has `null` values in its columns. `DataFrame` and `DataFrameColumn` offer an API to fill `nulls` with values.

``` csharp
df["Ints"].FillNulls(-1, inPlace: true);
df["Strings"].FillNulls("Bar", inPlace: true);
```

![Fill Nulls](FillNulls.PNG)

One of the design decisions we made early on was to use a column major backing store for `PrimitiveDataFrameColumn`. The column is held in memory as a collection of `Memory<byte>`. One consequence of this decision is that columns can have a length greater than `int.MaxValue`. The more important consequence is the ability to perform SIMD operations on our columns(we won't go through it in this sample, but the [unit tests](https://github.com/dotnet/corefxlab/blob/master/tests/Microsoft.Data.Analysis.Tests/BufferTests.cs#L200) show how to access the `Memory<byte>`). Indexing into our columns is therefore cheap, however, accessing our data row-wise is not.


`DataFrame` exposes a `Columns` property that we can enumerate over to access our columns. The `0.1.0` release does not expose a `Rows` property yet. Instead, we can directly index into our `DataFrame` to access our rows. Here's an example that accesses the first row:
```csharp
IList<object> row0 = df[0];
```
![Access Rows](RowAccess.PNG)

To enumerate over all the rows in a `DataFrame`, we can write a simple for loop. `DataFrame.RowCount` returns the length of a `DataFrame` and we can use the loop index to access each row. 
```csharp
for (long i = 0; i < df.RowCount; i++)
{
       IList<object> row = df[i];
}
```
Note that each row is returned as an `IList<object>`. Modifying the returned row does not modify the values in the `DataFrame` (use indexers for this instead). We also lose type information on the returned row object. This is a consequence of `DataFrame` being a loosely typed data structure. 

 Finally, let's wrap up by looking at the `Filter`, `Sort` and `GroupBy` methods:

``` csharp
// Filter rows based on equality
PrimitiveDataFrameColumn<bool> boolFilter = strings.ElementwiseEquals("Bar");
DataFrame filtered = df.Filter(boolFilter);
```
 ![DataFrame Filter](DataFrameFilter.PNG)

`ElementwiseEquals` returns a `PrimitiveDataFrameColumn<bool>` where each value in `strings` that equals `"Bar"` is set to `true`. In the `df.Filter` call, each row corresponding to a `true` value in `boolFilter` selects a row out of `df`. The resulting `DataFrame` contains only these rows.

```csharp
// Sort our dataframe using the Ints column
DataFrame sorted = df.Sort("Ints");
// GroupBy 
GroupBy groupBy = df.GroupBy("DateTimes");
```
![Sort and GroupBy](SortAndGroupBy.PNG)

The `GroupBy` method takes in the name of a column and creates groups based on unique values in the column. In our sample, the `DateTimes` column has 2 unique values, so we expect only 1 group to be created for `2019-01-01 00:00:00Z` and 1 for `2019-01-02 00:00:00Z`. 

``` csharp
// Count of values in each group
DataFrame groupCounts = groupBy.Count();
// Alternatively find the sum of the values in each group in Ints
DataFrame intsGroupSum = groupBy.Sum("Ints");
```
![GroupBy Sum](GroupBySum.PNG)

The `GroupBy` object exposes a set of methods that can called on each group. Some examples are `Max()`, `Min()`, `Count()` etc. The `Count()` method counts the number of values in each group and return them in a new `DataFrame`. The `Sum("Ints")` method sums up the values in each group. 

## Parting Thoughts

We've only explored a subset of the features that `DataFrame` exposes. `Joins`, `Merges`, and `Aggregations` are supported. Each column also implements `IEnumerable<T>`, so users can write LINQ queries on columns. The custom `DataFrame` formatting code we wrote has a simple example. `ApplyElementwise` is a method that is defined on `PrimitiveDataFrameColumn` that can take in a lambda to apply to each value. The complete source code(and documentation) for `Microsoft.Data.Analysis` lives [here](https://github.com/dotnet/corefxlab/tree/master/src/Microsoft.Data.Analysis). In a follow up post, I'll go over how to use `DataFrame` with ML.NET and .NET for Spark. The decision to use column major backing stores (the Arrow format in particular) allows for zero-copy in .NET for Spark User Defined Functions (UDFs)!

We always welcome the community's feedback! In fact, please feel free to contribute to the [source code](https://github.com/dotnet/corefxlab/tree/master/src/Microsoft.Data.Analysis). We've made it easy for users to create new column types that derive from `DataFrameColumn` to add new functionality. Support for structs such as `DateTime` and user defined structs is also not as complete as primitive types such as `int`, `float` etc. We believe this preview package allows the community to do data analysis in .NET. Give it a [try](https://github.com/dotnet/try/tree/master/NotebookExamples/csharp/Samples) and let us know your thoughts!

