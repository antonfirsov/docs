Automatic code formatter released to GitHub 
===

We have just released the [code formatting tool](https://github.com/dotnet/codeformatter) we use to automatically format our code to the prescribed [coding guidelines](https://github.com/dotnet/corefx/wiki/Contributing#c-coding-style).  This tool is based on Roslyn and will work on any C# project.

We strongly believe that having a consistent style greatly increases the readability and maintainability of a code base.  The individual style decisions themselves are subjective, the key is the consistency of the decisions.  As such the .NET team has a set of style guidelines for all C# code written by the team.  

Unfortunately these style decisions weren't defined from the beginning of .NET and as a result a good number of legacy code bases didn't follow them.  A coding style loses its benefits if it's not followed throughout the project and many of these non-conformant code bases represented some of our most core BCL projects.

This was a long standing issue on the team that just never quite met the bar for fixing.  Why waste a developer's time editing thousands and thousands of C# files when there are all of these real bugs to fix?

This changed when the decision was made to open source the code.  The internal team was used to the inconsistencies but did we want to force it on our developer community?  What would our contribution guidelines look like in a world where some of our most core libraries had a different style than the one we wanted to recommend?  That didn't seem like a great story for customers and we decided to fix it with tooling.

Luckily the Roslyn project presented us with the tools to fix this problem.  Its core features include a full fidelity parse tree and a set of APIs to safely manipulate it syntactically or semantically.  This is exactly what we needed to migrate all of that legacy source into a single unified style.

The tool itself is very simple to use; point it at a project or solution and it will systematically convert all of the code involved into the prescribed coding style.  The process is very fast, taking only a few seconds for most projects and up to a couple of minutes for very large ones.  It can even be run repeatedly on them to ensure that a consistent style is maintained over the course of time.

Take for example how easy it was to fix up the Regex code base which has been around since 1.0:

``` 
codeformatter.exe System.Text.RegularExpressions.csproj
```

That took code with old stanards and documentation like the folowing from Regex.cs:

``` csharp
/// <devdoc>
///    Returns 
///       the GroupNameCollection for the regular expression. This collection contains the
///       set of strings used to name capturing groups in the expression. 
///    </devdoc>
public String[] GetGroupNames() {
    String[] result;

    if (capslist == null) {
        int max = capsize;
        result = new String[max];

        for (int i = 0; i < max; i++) {
            result[i] = Convert.ToString(i, CultureInfo.InvariantCulture);
        }
    }
    else {
        result = new String[capslist.Length];

        System.Array.Copy(capslist, 0, result, 0, capslist.Length);
    }

    return result;
}
```

And converted it to the following:

``` csharp       
/// <summary>
/// Returns the GroupNameCollection for the regular expression. This collection contains the
/// set of strings used to name capturing groups in the expression.
/// </summary>
public String[] GetGroupNames()
{
    String[] result;

    if (_capslist == null)
    {
        int max = _capsize;
        result = new String[max];

        for (int i = 0; i < max; i++)
        {
            result[i] = Convert.ToString(i, CultureInfo.InvariantCulture);
        }
    }
    else
    {
        result = new String[_capslist.Length];

        System.Array.Copy(_capslist, 0, result, 0, _capslist.Length);
    }

    return result;
}
```

Right now the tool is a library with a simple command line wrapper.  Going forward we will be looking to support a number of other scenarios including working inside of Visual Studio.  Why should developers ever think about mindless style edits when the tool can just fix up the style differences as you code?

This tool is now on GitHub in the [codeformatter repo](https://github.com/dotnet/codeformatter).  We appreciate any feedback from the community or even better some contributions! 

Note: This tool has the potential to cause a lot of churn on a code base.  Best practice would be to communicate the desire to update the style with the project owner before sending them a large style PR.  
