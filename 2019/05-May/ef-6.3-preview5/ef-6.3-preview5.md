# Announcing Entity Framework 6.3 Preview with .NET Core Support
The first preview of the EF 6.3 runtime is now [available in NuGet](https://www.nuget.org/packages/EntityFramework/6.3.0-preview5-19254-05). 

Note that the package is versioned as 6.3.0-preview5. We plan to continue releasing previews of EF 6.3 every month in alignment with the .NET Core 3.0 previews, until we ship the final version.

## What is new in EF 6.3?

While Entity Framework Core was built from the ground up to work on .NET Core, EF 6.3 will be the first version of EF 6 that can run on .NET Core and work cross-platform. In fact, the main goal of this release is to facilitate migrating existing applications that use EF 6 to .NET Core 3.0. 

When completed, EF 6.3 will also have support for:

- NuGet PackageReferences (this implies working smoothly without any EF entries in application config files) 
- Migration commands running on projects using the the new .NET project system 

Besides these, so far we have incorporated around 10 other bug fixes and community contributions. These apply to EF 6 when running on both .NET Core and .NET Framework. You can learn more about them [in our GitHub repo](https://github.com/aspnet/EntityFramework6/issues?utf8=%E2%9C%93&q=is%3Aissue+milestone%3A6.3.0-preview5+is%3Aclosed+-label%3Aarea-test+-label%3Atype-cleanup).

## Known limitations

Although this preview makes it possible to start using the EF 6 runtime on .NET Core 3.0, it still has major limitations. For example:

- Migration commands cannot be executed on projects not targeting .NET Framework.
- There is no EF designer support for projects not targeting .NET Framework.
- There is no support for building and running with models based on EDMX files on .NET Core. On .NET Framework, this depends on a build task that splits and embeds the contents of the EDMX file into the final executable file, and that task is not available for .NET Core.
- Running code first projects in .NET Core is easier but still requires additional steps, like registering DbProviderFactories programmatically, and either passing the connection string explicitly, or setting up a DbConnectionFactory in a DbConfiguration.
- Only the SQL Server provider, based on System.Data.SqlClient, works on .NET Core. Other EF6 providers with support for .NET Core haven't been released yet.

Besides these temporary limitations, there are other longer term ones:

- We have no plans to support the SQL Server Compact provider on .NET Core. There is no ADO.NET provider for SQL Server Compact on .NET Core.
- SQL Server spatial types and HierarchyID aren't available on .NET Core.

## Getting started using EF 6.3 on .NET Core 3.0

You will need to download and install the [.NET Core 3.0 preview 5 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.0). Once you have done that, you can use Visual Studio to create a Console .NET Core 3.0 application and install the EF 6.3 preview package from NuGet:

``` powershell
> Install-Package EntityFramework -pre
``` 

Edit the `Program.cs` file in the application to look tike this:

``` csharp
using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Common;
using System.Data.SqlClient;

namespace TryEF6OnCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"server=(localdb)\mssqllocaldb; database=MyContext; Integrated Security=true; ConnectRetryCount=0";

            // workaround:
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

            using (var db = new MyContext(connectionString))
            {

                db.Database.CreateIfNotExists();
                db.People.Add(new Person { Name = "Diego" });
                db.SaveChanges();
            }

            using (var db = new MyContext(connectionString))
            {
                Console.WriteLine($"{db.People.First()?.Name} wrote this sample");
            }
        }
    }

    public class MyContext : DbContext
    {
        public MyContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<Person> People { get; set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}

```

## Closing

We would like to encourage you to download the preview package and try the code first experience on .NET Core and the complete set of scenarios on .NET Framework. Please, report any issues you find to [our GitHub repo](https://github.com/aspnet/EntityFramework6/issues/new). 