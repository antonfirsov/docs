---
post_title: 'Trying out MongoDB with EF Core using Testcontainers'
author1: avickers@microsoft.com
post_slug: efcore-mongodb
microsoft_alias: avickers
featured_image: efcoremongo.jpg
categories: .NET, Entity Framework
summary: An introduction to the MongoDB database provider for EF Core, including use of Testcontainers
desired_publication_date: 2023-11-02
tags: ef core, efcore, entity framework, mongodb
post_date: 2023-11-02 08:00:00
---

Helping developers use both relational _and non-relational_ databases effectively was one of the original tenets of EF Core. To this end, there has been an [EF Core database provider for Azure Cosmos DB document databases](https://learn.microsoft.com/ef/core/providers/cosmos/) for many years now. Recently, the EF Core team has been collaborating with engineers from [MongoDB](https://www.mongodb.com/) to bring support for MongoDB to EF Core. The initial result of this collaboration is the first [preview release of the MongoDB provider for EF Core](https://www.mongodb.com/blog/post/mongodb-provider-entity-framework-core-now-available-public-preview).

In this post, we will try out [the MongoDB provider](https://github.com/mongodb/mongo-efcore-provider) for EF Core by using it to:

- Map a C# object model to documents in a MongoDB database
- Use EF to save some documents to the database
- Write LINQ queries to retrieve documents from the database
- Make changes to a document and use EF’s change tracking to update the document

The code shown in this post [can be found on GitHub](https://github.com/ajcvickers/MongoTestApp).

## Testcontainers

It’s very easy to [get a MongoDB database in the cloud](https://www.mongodb.com/atlas/database) that you can use to try things out. However, [Testcontainers](https://testcontainers.com/) is another way to test code with different database systems which is particularly suited to:

- Running automated tests against the database
- Creating standalone reproductions when reporting issues
- Trying out new things with minimal setup

Testcontainers are distributed as NuGet packages that take care of running a container containing a configured ready-to-use database system. The containers use Docker or a Docker-alternative to run, so this may need to be installed on your machine if you don’t already have it. See [_Welcome to Testcontainers for .NET!_](https://dotnet.testcontainers.org/) for more details. Other than starting Docker, you don't need to do anything else except import the NuGet package.

## The C# project

We'll use a simple console application to try out MongoDB with EF Core. This project needs two package references:

- [MongoDB.EntityFrameworkCore](https://www.nuget.org/packages/MongoDB.EntityFrameworkCore) to install the EF Core provider. This package also transitives installs the common EF Core packages and the [MongoDB.Driver](https://www.nuget.org/packages/MongoDB.Driver/) package which is used by the EF Provider to access the MongoDB database.
- [Testcontainers.MongoDb](https://www.nuget.org/packages/Testcontainers.MongoDb) to install the pre-defined Testcontainer for MongoDB.

The full `csproj` file looks like this:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net7.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <RootNamespace />
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Testcontainers.MongoDB" Version="3.5.0" />
        <PackageReference Include="MongoDB.EntityFrameworkCore" Version="7.0.0-preview.1" />
    </ItemGroup>

</Project>
```

Remember, the full project is available to [download from GitHUb](https://github.com/ajcvickers/MongoTestApp).

## The object model

We'll map a simple object model of customers and their addresses:

```csharp
public class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required Species Species { get; set; }
    public required ContactInfo ContactInfo { get; set; }
}

public class ContactInfo
{
    public required Address ShippingAddress { get; set; }
    public Address? BillingAddress { get; set; }
    public required PhoneNumbers Phones { get; set; }
}

public class PhoneNumbers
{
    public PhoneNumber? HomePhone { get; set; }
    public PhoneNumber? WorkPhone { get; set; }
    public PhoneNumber? MobilePhone { get; set; }
}

public class PhoneNumber
{
    public required int CountryCode { get; set; }
    public required string Number { get; set; }
}

public class Address
{
    public required string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Line3 { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string PostalCode { get; set; }
}

public enum Species
{
    Human,
    Dog,
    Cat
}
```

Since MongoDB works with documents, we're going to map this model to a top level Customer document, with the addresses and phone numbers embedded in this document. We'll see how to do this in the next section.

## Creating the EF model

EF works by [building a model of the mapped CLR types](https://learn.microsoft.com/ef/core/modeling/), such as those for `Customer`, etc. in the previous section. This model defines the relationships between types in the model, as well as how each type maps to the database. 

Luckily there is not much to do here, since EF uses a set of model building conventions that generate a model based on input from both the model types and the database provider. This means that for relational databases, each type gets mapped to a different table by convention. For document databases like Azure CosmosDB and now MongoDB, only the top-level type (`Customer` in our example) is mapped to its own document. Other types referenced from the top-level types are, by-convention, included in the main document.

This means that the only thing EF needs to know to build a model is the top-level type, and that the MongoDB provider should be used. We do this by defining a type that extends from `DbContext`. For example:

```csharp
public class CustomersContext : DbContext
{
    private readonly MongoClient _client;

    public CustomersContext(MongoClient client)
    {
        _client = client;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMongoDB(_client, "efsample");

    public DbSet<Customer> Customers => Set<Customer>();
}
```

In this `DbContext` class:

- `UseMongoDB` is called, passing in the client driver and the database name. This tells EF Core to use the MongoDB provider when building the model and accessing the database.
- A `DbSet<Customer>` property that defines the top-level type for which documents should be modeled.

We'll see later how to create the `MongoClient` instance and use the `DbContext`. When we do, examining the [model DebugView](https://learn.microsoft.com/ef/core/modeling/#debug-view) shows this:

```text
Model: 
  EntityType: ContactInfo Owned
    Properties:
      CustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
    Navigations:
      BillingAddress (Address) ToDependent ContactInfo.BillingAddress#Address (Address)
      Phones (PhoneNumbers) ToDependent PhoneNumbers
      ShippingAddress (Address) ToDependent ContactInfo.ShippingAddress#Address (Address)
    Keys:
      CustomerId PK
    Foreign keys:
      ContactInfo {'CustomerId'} -> Customer {'Id'} Unique Ownership ToDependent: ContactInfo Cascade
  EntityType: ContactInfo.BillingAddress#Address (Address) CLR Type: Address Owned
    Properties:
      ContactInfoCustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
      City (string) Required
      Country (string) Required
      Line1 (string) Required
      Line2 (string)
      Line3 (string)
      PostalCode (string) Required
    Keys:
      ContactInfoCustomerId PK
    Foreign keys:
      ContactInfo.BillingAddress#Address (Address) {'ContactInfoCustomerId'} -> ContactInfo {'CustomerId'} Unique Ownership ToDependent: BillingAddress Cascade
  EntityType: ContactInfo.ShippingAddress#Address (Address) CLR Type: Address Owned
    Properties:
      ContactInfoCustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
      City (string) Required
      Country (string) Required
      Line1 (string) Required
      Line2 (string)
      Line3 (string)
      PostalCode (string) Required
    Keys:
      ContactInfoCustomerId PK
    Foreign keys:
      ContactInfo.ShippingAddress#Address (Address) {'ContactInfoCustomerId'} -> ContactInfo {'CustomerId'} Unique Ownership ToDependent: ShippingAddress Cascade
  EntityType: Customer
    Properties:
      Id (Guid) Required PK AfterSave:Throw ValueGenerated.OnAdd
      Name (string) Required
      Species (Species) Required
    Navigations:
      ContactInfo (ContactInfo) ToDependent ContactInfo
    Keys:
      Id PK
  EntityType: PhoneNumbers Owned
    Properties:
      ContactInfoCustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
    Navigations:
      HomePhone (PhoneNumber) ToDependent PhoneNumbers.HomePhone#PhoneNumber (PhoneNumber)
      MobilePhone (PhoneNumber) ToDependent PhoneNumbers.MobilePhone#PhoneNumber (PhoneNumber)
      WorkPhone (PhoneNumber) ToDependent PhoneNumbers.WorkPhone#PhoneNumber (PhoneNumber)
    Keys:
      ContactInfoCustomerId PK
    Foreign keys:
      PhoneNumbers {'ContactInfoCustomerId'} -> ContactInfo {'CustomerId'} Unique Ownership ToDependent: Phones Cascade
  EntityType: PhoneNumbers.HomePhone#PhoneNumber (PhoneNumber) CLR Type: PhoneNumber Owned
    Properties:
      PhoneNumbersContactInfoCustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
      CountryCode (int) Required
      Number (string) Required
    Keys:
      PhoneNumbersContactInfoCustomerId PK
    Foreign keys:
      PhoneNumbers.HomePhone#PhoneNumber (PhoneNumber) {'PhoneNumbersContactInfoCustomerId'} -> PhoneNumbers {'ContactInfoCustomerId'} Unique Ownership ToDependent: HomePhone Cascade
  EntityType: PhoneNumbers.MobilePhone#PhoneNumber (PhoneNumber) CLR Type: PhoneNumber Owned
    Properties:
      PhoneNumbersContactInfoCustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
      CountryCode (int) Required
      Number (string) Required
    Keys:
      PhoneNumbersContactInfoCustomerId PK
    Foreign keys:
      PhoneNumbers.MobilePhone#PhoneNumber (PhoneNumber) {'PhoneNumbersContactInfoCustomerId'} -> PhoneNumbers {'ContactInfoCustomerId'} Unique Ownership ToDependent: MobilePhone Cascade
  EntityType: PhoneNumbers.WorkPhone#PhoneNumber (PhoneNumber) CLR Type: PhoneNumber Owned
    Properties:
      PhoneNumbersContactInfoCustomerId (no field, Guid) Shadow Required PK FK AfterSave:Throw
      CountryCode (int) Required
      Number (string) Required
    Keys:
      PhoneNumbersContactInfoCustomerId PK
    Foreign keys:
      PhoneNumbers.WorkPhone#PhoneNumber (PhoneNumber) {'PhoneNumbersContactInfoCustomerId'} -> PhoneNumbers {'ContactInfoCustomerId'} Unique Ownership ToDependent: WorkPhone Cascade
```

Looking at this model, it can be seen that EF created [owned entity types](https://learn.microsoft.com/ef/core/modeling/owned-entities) for the `ContactInfo`, `Address`, `PhoneNumber` and `PhoneNumbers` types, even though only the `Customer` type was referenced directly from the `DbContext`. These other types were discovered and configured by the model-building conventions.

## Create the MongoDB test container

We now have a model and a `DbContext`. Next we need an actual MongoDB database, and this is where Testcontainers come in. There are Testcontainers available for many different types of database, and they all work in a very similar way. That is, a container is created using the appropriate `DbBuilder`, and then that container is started. For example:

```csharp
await using var mongoContainer = new MongoDbBuilder()
    .WithImage("mongo:6.0")
    .Build();

await mongoContainer.StartAsync();
```

And that's it! We now have a configured, clean MongoDB instance running locally with which we can do what we wish, before just throwing it away.

## Save data to MongoDB

Let's use EF Core to write some data to the MongoDB database. To do this, we'll need to create a `DbContext` instance, and for this we need a `MongoClient` instance from the underlying MongoDB driver. Often, in a real app, the `MongoClient` instance and the `DbContext` instance will be obtained using dependency injection. For the sake of simplicity, we'll just `new` them up here:

```csharp
var mongoClient = new MongoClient(mongoContainer.GetConnectionString());

await using (var context = new CustomersContext(mongoClient))
{
    // ...
}
```

Notice that the Testcontainer instance provides the connection string we need to connect to our MongoDB test database.

To save a new `Customer` document, we'll use `Add` to start tracking the document, and then call `SaveChangesAsync` to insert it into the database.

```csharp
await using (var context = new CustomersContext(mongoClient))
{
    var customer = new Customer
    {
        Name = "Willow",
        Species = Species.Dog,
        ContactInfo = new()
        {
            ShippingAddress = new()
            {
                Line1 = "Barking Gate",
                Line2 = "Chalk Road",
                City = "Walpole St Peter",
                Country = "UK",
                PostalCode = "PE14 7QQ"
            },
            BillingAddress = new()
            {
                Line1 = "15a Main St",
                City = "Ailsworth",
                Country = "UK",
                PostalCode = "PE5 7AF"
            },
            Phones = new()
            {
                HomePhone = new() { CountryCode = 44, Number = "7877 555 555" },
                MobilePhone = new() { CountryCode = 1, Number = "(555) 2345-678" },
                WorkPhone = new() { CountryCode = 1, Number = "(555) 2345-678" }
            }
        }
    };
    
    context.Add(customer);
    await context.SaveChangesAsync();
}
```

If we look at the JSON (actually, [BSON](https://bsonspec.org/spec.html), which is a more efficient binary representation for JSON documents) document created in the database, we can see it contains nested documents for all the contact information. This is different from what EF Core would do for a relational database, where each type would have been mapped to its own top-level table.

```json
{
  "_id": "CSUUID(\"9a97fd67-515f-4586-a024-cf82336fc64f\")",
  "Name": "Willow",
  "Species": 1,
  "ContactInfo": {
    "BillingAddress": {
      "City": "Ailsworth",
      "Country": "UK",
      "Line1": "15a Main St",
      "Line2": null,
      "Line3": null,
      "PostalCode": "PE5 7AF"
    },
    "Phones": {
      "HomePhone": {
        "CountryCode": 44,
        "Number": "7877 555 555"
      },
      "MobilePhone": {
        "CountryCode": 1,
        "Number": "(555) 2345-678"
      },
      "WorkPhone": {
        "CountryCode": 1,
        "Number": "(555) 2345-678"
      }
    },
    "ShippingAddress": {
      "City": "Walpole St Peter",
      "Country": "UK",
      "Line1": "Barking Gate",
      "Line2": "Chalk Road",
      "Line3": null,
      "PostalCode": "PE14 7QQ"
    }
  }
}
```

## Using LINQ queries

EF Core supports [LINQ for querying data](https://learn.microsoft.com/ef/core/querying/). For example, to query a single customer:

```csharp
using (var context = new CustomersContext(mongoClient))
{
    var customer = await context.Customers.SingleAsync(c => c.Name == "Willow");

    var address = customer.ContactInfo.ShippingAddress;
    var mobile = customer.ContactInfo.Phones.MobilePhone;
    Console.WriteLine($"{customer.Id}: {customer.Name}");
    Console.WriteLine($"    Shipping to: {address.City}, {address.Country} (+{mobile.CountryCode} {mobile.Number})");
}
```

Running this code results in the following output:

```text
336d4936-d048-469e-84c8-d5ebc17754ff: Willow
    Shipping to: Walpole St Peter, UK (+1 (555) 2345-678)
```

Notice that the query pulled back the entire document, not just the `Customer` object, so we are able to access and print out the customer's contact info without going back to the database.

Other LINQ operators can be used to perform filtering, etc. For example, to bring back all customers where the `Species` is `Dog`:

```csharp
var customers = await context.Customers
    .Where(e => e.Species == Species.Dog)
    .ToListAsync();
```

## Updating a document

By default, [EF tracks the object graphs returned from queries](https://learn.microsoft.com/ef/core/querying/tracking). Then, when `SaveChanges` or `SaveChangesAsync` is called, EF detects any changes that have been made to the document and sends an update to MongoDB to update that document. For example:

```csharp
using (var context = new CustomersContext(mongoClient))
{
    var baxter = (await context.Customers.FindAsync(baxterId))!;
    baxter.ContactInfo.ShippingAddress = new()
    {
        Line1 = "Via Giovanni Miani",
        City = "Rome",
        Country = "IT",
        PostalCode = "00154"
    };
    
    await context.SaveChangesAsync();
}
```

In this case, we're using `FindAsync` to query a customer by primary key--a LINQ query would work just as well. After that, we change the shipping address to Rome, and call `SaveChangesAsync`. EF detects that only the shipping address for a single document has been changed, and so sends a partial update to patch the updated address into the document stored in the MongoDB database.

## Going forward

So far, the MongoDB provider for EF Core is only in its first preview. Full CRUD (creating, reading, updating, and deleting documents) is supported by this preview, but there are some limitations. See the [readme](https://github.com/mongodb/mongo-efcore-provider#readme) on GitHub for more information, and for places to ask questions and file bugs.

## Learn more

To learn more about EF Core and MongoDB:

- See the [EF Core documentation](https://learn.microsoft.com/ef/core/) to learn more about using EF Core to access all kinds of databases.
- See the [MongoDB documentation](https://www.mongodb.com/docs/) to learn more about using MongoDB from any platform.
- Watch [Introducing the MongoDB provider for EF Core](https://www.youtube.com/live/Zat-ferrjro?si=WD2mjd2_dd3dquko) on the .NET Data Community Standup.
- Watch the upcoming [Announcing MongoDB Provider for Entity Framework Core](https://www.youtube.com/live/EQvRGG9Ine8?si=TkN5q_4ZiHPoxkJy) on the MongoDB livestream.
 
## Summary

We used [Testcontainers](https://testcontainers.com/) to try out the [first preview release of the MongoDB provider for EF Core](https://github.com/mongodb/mongo-efcore-provider). Testcontainers allowed us to test MongoDB with very minimal setup, and we were able to create, query, and update documents in the MongoDB database using EF Core.
