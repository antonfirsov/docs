At Spark + AI Summit in [May 2019][1], we released [.NET for Apache Spark][2]. .NET for Apache Spark is aimed at making [Apache® Spark™][3], and thus the exciting world of big data analytics, accessible to .NET developers.

.NET for Spark can be used for processing batches of data, real-time streams, machine learning, and ad-hoc query. In this blog post, we’ll explore how to use .NET for Spark to perform a very popular big data task known as **log analysis**.

<img class="" src="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2019/04/dotnetsparklogo-6.png" alt=".NET Spark Logo" width="906" height="334" border="2" />

The remainder of this post describes the following topics:

*   [What is log analysis?][4]
*   [Writing a .NET for Spark log analysis app][5]
*   [Running a .NET for Spark app][6]
*   [Wrap Up][7]

### <a id="whatislog"></a>What is log analysis?

Log analysis, also known as *log processing*, is the process of analyzing computer-generated records called logs. Logs tell us what’s happening on a tool like a computer or web server, such as what applications are being used or the top websites users visit.

The goal of log analysis is to gain meaningful insights from these logs about activity and performance of our tools or services. .NET for Spark enables us to analyze anywhere from megabytes to petabytes of log data with blazing fast and efficient processing!

In this blog post, we’ll be analyzing a set of [Apache log entries][8] that express how users are interacting with content on a web server. You can view a sample of Apache log entries [here][9].

### <a id="sparkforlog"></a>Writing a .NET for Spark log analysis app

Log analysis is an example of [batch processing][10] with Spark. Batch processing is the transformation of data at rest, meaning that the source data has already been loaded into data storage. In our case, the input text file is already populated with logs and won’t be receiving new or updated logs as we process it.

When creating a new .NET for Spark application, there are just a few steps we need to follow to start getting those interesting insights from our data:

1.  Create a Spark Session.
2.  Read input data, typically using a DataFrame.
3.  Manipulate and analyze input data, typically using Spark SQL.

#### <a id="createsession"></a> Create a Spark Session

In any Spark application, we start off by establishing a new [SparkSession][11], which is the entry point to programming with Spark:

<pre class="prettyprint">SparkSession spark = SparkSession
    .Builder()
    .AppName("Apache User Log Processing")
    .GetOrCreate();</pre>

By calling on the `spark` object created above, we can now access Spark and DataFrame functionality throughout our program – great! But what is a DataFrame? Let’s learn about it in the next step.

#### <a id="readindata"></a> Read input data

Now that we have access to Spark functionality, we can read in the log data we’ll be analyzing. We store input data in a [DataFrame][12], which is a distributed collection of data organized into named columns:

<pre class="prettyprint">DataFrame generalDf = spark.Read().Text("&lt;path to input data set>");</pre>

When our input is contained in a *.txt* file, we use the `.Text()` method, as shown above. There are [other methods][13] to read in data from other sources, such as `.Csv()` to read in comma-separated values files.

#### <a id="manipulatedata"></a> Manipulate and analyze input data

With our input logs stored in a DataFrame, we can start analyzing them – now things are getting exciting!

An important first step is **data preparation**. Data prep involves cleaning up our data in some way. This could include removing incomplete entries to avoid error in later calculations or removing irrelevant input to improve performance.

In our example, we should first ensure all of our entries are complete logs. We can do this by comparing each log entry to a [regular expression][14] (AKA a regex), which is a sequence of characters that defines a pattern.

Let’s define a regex expressing a pattern all valid Apache log entries should follow:

<pre class="prettyprint">string s_apacheRx = "^(\S+) (\S+) (\S+) [([\w:/]+\s[+-]\d{4})] \"(\S+) (\S+) (\S+)\" (\d{3}) (\d+)";</pre>

How do we perform a calculation on each row of a DataFrame, like comparing each log entry to the above regex? The answer is *Spark SQL*.

#### Spark SQL

Spark SQL provides many great functions for working with the structured data stored in a DataFrame. One of the most popular features of Spark SQL is *UDFs*, or user-defined functions. We define the type of input they take and the type of output they produce, and then the actual calculation or filtering they perform.

Let’s define a new UDF `GeneralReg` to compare each log entry to the `s_apacheRx` regex. Our UDF requires an Apache log entry, which is a string, and will return a true or false depending upon if the log matches the regex:

<pre class="prettyprint">spark.Udf().Register&lt;string, bool>("GeneralReg", log => Regex.IsMatch(log, s_apacheRx));</pre>

So how do we call `GeneralReg`?

In addition to UDFs, Spark SQL provides the ability to write **SQL calls** to analyze our data – how convenient! It’s common to write a SQL call to apply a UDF to each row of data.

To call `GeneralReg` from above, let’s use the following SQL call:

<pre class="prettyprint">DataFrame generalDf = spark.Sql("SELECT logs.value, GeneralReg(logs.value) FROM Logs");</pre>

This SQL call tests each row of `generalDf` to determine if it’s a valid and complete log.

We can use [.Filter()][15] to only keep the complete log entries in our data, and then [.Show()][16] to display our newly filtered DataFrame:

<pre class="prettyprint">generalDf = generalDf.Filter(generalDf[“GeneralReg(value)”]);
generalDf.Show();</pre>

Now that we’ve performed some initial data prep, we can continue filtering and analyzing our data. Let’s find log entries from IP addresses starting with 10 and related to spam in some way:

<pre class="prettyprint">// Choose valid log entries that start with 10
spark.Udf().Register&lt;string, bool>(
    "IPReg",
    log => Regex.IsMatch(log, "^(?=10)"));

generalDf.CreateOrReplaceTempView("IPLogs");

// Apply UDF to get valid log entries starting with 10
DataFrame ipDf = spark.Sql(
    "SELECT iplogs.value FROM IPLogs WHERE IPReg(iplogs.value)");
ipDf.Show();

// Choose valid log entries that start with 10 and deal with spam
spark.Udf().Register&lt;string, bool>(
    "SpamRegEx",
    log => Regex.IsMatch(log, "\\b(?=spam)\\b"));

ipDf.CreateOrReplaceTempView("SpamLogs");

// Apply UDF to get valid, start with 10, spam entries
DataFrame spamDF = spark.Sql(
    "SELECT spamlogs.value FROM SpamLogs WHERE SpamRegEx(spamlogs.value)");</pre>

Finally, let’s count the number of GET requests in our final cleaned dataset. The magic of .NET for Spark is that we can combine it with other popular .NET features to write our apps. We’ll use [LINQ][17] to analyze the data in our Spark app one last time:

<pre class="prettyprint">int numGetRequests = spamDF 
    .Collect() 
    .Where(r => ContainsGet(r.GetAs&lt;string>("value"))) 
    .Count();</pre>

In the above code, `ContainsGet()` checks for GET requests using [regex matching][18]:

<pre class="prettyprint">// Use regex matching to group data 
// Each group matches a column in our log schema 
// i.e. first group = first column = IP
public static bool ContainsGet(string logLine) 
{ 
    Match match = Regex.Match(logLine, s_apacheRx);

    // Determine if valid log entry is a GET request
    if (match.Success)
    {
        Console.WriteLine("Full log entry: '{0}'", match.Groups[0].Value);
    
        // 5th column/group in schema is "method"
        if (match.Groups[5].Value == "GET")
        {
            return true;
        }
    }

    return false;

} </pre>

As a final step in our Spark apps, we call `spark.Stop()` to shut down the underlying Spark Session and Spark Context.

You can view the [complete log processing example][19] in our GitHub repo.

### <a id="running"></a> Running your app

[To run a .NET for Apache Spark app][20], you need to use the `spark-submit` command, which will submit your application to run on Apache Spark.

The main parts of `spark-submit` include:

*   --class, to call the DotnetRunner.
*   --master, to determine if this is a local or cloud Spark submission.
*   Path to the Microsoft.Spark jar file.
*   Any arguments to your app, such as the path to your input file.

You’ll also need to download and setup some dependencies before running a .NET for Spark app locally, such as Java and Apache Spark.

A sample Windows command for running your app is as follows:

`spark-submit --class org.apache.spark.deploy.dotnet.DotnetRunner --master local /path/to/microsoft-spark-<version>.jar /path/to/access_log.txt`

### <a id="wrapup"></a>Wrap Up

We’d love to help you get started with .NET for Apache Spark and hear your feedback.

You can [Request a Demo][2] from our landing page and check out the [.NET for Spark GitHub repo][21] to learn more about how you can apply .NET for Spark in your apps and get involved with our effort to make .NET a great tech stack for building big data applications!

 [1]: https://devblogs.microsoft.com/dotnet/introducing-net-for-apache-spark/
 [2]: https://dot.net/spark
 [3]: https://spark.apache.org/
 [4]: #whatislog
 [5]: #sparkforlog
 [6]: #running
 [7]: #wrapup
 [8]: https://httpd.apache.org/docs/1.3/logs.html
 [9]: https://raw.githubusercontent.com/elastic/examples/master/Common%20Data%20Formats/apache_logs/apache_logs
 [10]: https://docs.microsoft.com/en-us/dotnet/spark/tutorials/batch-processing
 [11]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.spark.sql.sparksession?view=spark-dotnet
 [12]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.spark.sql.dataframe?view=spark-dotnet
 [13]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.spark.sql.dataframereader?view=spark-dotnet#methods
 [14]: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
 [15]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.spark.sql.dataframe.filter?view=spark-dotnet
 [16]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.spark.sql.dataframe.show?view=spark-dotnet
 [17]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/
 [18]: https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match?view=netframework-4.8
 [19]: https://github.com/dotnet/spark/blob/master/examples/Microsoft.Spark.CSharp.Examples/Sql/Batch/Logging.cs
 [20]: https://docs.microsoft.com/en-us/dotnet/spark/tutorials/get-started
 [21]: https://github.com/dotnet/spark
