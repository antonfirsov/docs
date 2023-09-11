---
post_title: 'EF Core 8 RC1: Complex types as value objects'
author1: avickers@microsoft.com
post_slug: announcing-ef8-rc1
microsoft_alias: avickers
featured_image: ef8rc1.png
categories: .NET, Entity Framework
summary: Announcing Entity Framework Core 8 (EF8) RC 1 with support for complex types used as value objects
desired_publication_date: 2023-09-12
tags: .net 8, ef core, efcore, ef8, entity framework
post_date: 2023-09-12 10:10:00
---

The first release candidate of Entity Framework Core (EF Core) 8 is [available on NuGet today](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/8.0.0-rc.1.23453.1)!

## Basic information

EF Core 8, or just EF8, is the successor to EF Core 7, and is scheduled for release in November 2023, at the same time as .NET 8.

EF8 requires .NET 8 and this RC1 release should be used with the [.NET 8 RC1 SDK](https://dotnet.microsoft.com/next).

EF8 will align with .NET 8 as a long-term support (LTS) release. See the [.NET support policy](https://dotnet.microsoft.com/platform/support/policy/dotnet-core) for more information.

## New in EF8

EF8 RC1 contains all the major feature features we intend to ship in EF8, although further tweaks and bug fixes are coming for RC2. These features include:

- [Analyzer: warn (and code fix) for use of interpolation in SQL methods accepting raw strings](https://github.com/dotnet/efcore/issues/30965)
- [Translate Contains to IN with subquery instead of EXISTS where relevant](https://github.com/dotnet/efcore/issues/30955)
- [Allow inline primitive collections with parameters, translating to VALUES](https://github.com/dotnet/efcore/issues/30732)
- [Translate DateOnly.FromDateTime](https://github.com/dotnet/efcore/issues/30708)
- [Implement JSON serialization/deserialization via Utf8JsonReader/Utf8JsonWriter](https://github.com/dotnet/efcore/issues/30604)
- [Update pattern for scaffolding column default constraints](https://github.com/dotnet/efcore/issues/13613)
- [Use IN instead of EXISTS with ExecuteDelete and entity containment](https://github.com/dotnet/efcore/issues/31386)
- [Allow ExecuteUpdate to update properties of multiple queries as long as the map to a single table](https://github.com/dotnet/efcore/issues/31406)
- [Query: add support for projecting primitive collections from JSON entities](https://github.com/dotnet/efcore/issues/31364)
- [Switch to storing enums as ints in JSON instead of strings](https://github.com/dotnet/efcore/issues/31100)
- [Translate DegreesToRadians](https://github.com/dotnet/efcore/issues/30926)
- [Metadata and type mapping support for primitive collections](https://github.com/dotnet/efcore/issues/30730)
- [JSON type representations and conversions to store types](https://github.com/dotnet/efcore/issues/30727)
- [Allow stripping away all model building code to reduce application size](https://github.com/dotnet/efcore/issues/29755)
- [Json: add support for collection of primitive types inside JSON columns](https://github.com/dotnet/efcore/issues/28688)
- [Support LINQ querying of non-primitive collections within JSON](https://github.com/dotnet/efcore/issues/28616)
- [SQLite RevEng: Sample data to determine CLR type](https://github.com/dotnet/efcore/issues/8824)
- [Allow default value check in value generation to be customized](https://github.com/dotnet/efcore/issues/701)
- [Update handling of non-nullable store-generated properties](https://github.com/dotnet/efcore/issues/15070)
- [IN() list queries are not parameterized, causing increased SQL Server CPU usage](https://github.com/dotnet/efcore/issues/13617)
- [Allow 'unsharing' connection between contexts](https://github.com/dotnet/efcore/issues/30704)
- [Remove unneeded subquery and projection when using ordering without limit/offset in set operations](https://github.com/dotnet/efcore/issues/30684)
- [Make SequentialGuidValueGenerator non-allocating](https://github.com/dotnet/efcore/issues/30610)
- [Support querying over primitive collections](https://github.com/dotnet/efcore/issues/30426)
- [JSON/Sqlite: use -> and ->> where possible when traversing JSON, rather than json_extract](https://github.com/dotnet/efcore/issues/30334)
- [Add Generic version of EntityTypeConfiguration Attribute](https://github.com/dotnet/efcore/issues/30072)
- [NativeAOT/trimming compatibility for Microsoft.Data.Sqlite](https://github.com/dotnet/efcore/issues/29725)
- [Map collections of primitive types to JSON column in relational database](https://github.com/dotnet/efcore/issues/29427)
- [Translate DateTimeOffset.ToUnixTime(Seconds|Milliseconds)](https://github.com/dotnet/efcore/issues/28925)
- [Allow pooling DbContext with singleton services](https://github.com/dotnet/efcore/issues/27752)
- [Optional RestartSequenceOperation.StartValue](https://github.com/dotnet/efcore/issues/26560)
- [Generate compiled relational model](https://github.com/dotnet/efcore/issues/24896)
- [Global query filters produce too many parameters](https://github.com/dotnet/efcore/issues/24476)
- [Optimize update path for single property JSON element](https://github.com/dotnet/efcore/issues/30410)
- [JSON columns can be used in compiled models](https://github.com/dotnet/efcore/issues/29602)
- [Unneeded parentheses removed in SQL queries ](https://github.com/dotnet/efcore/issues/26767)
- [Set operations are supported over non-entity projections with different facets](https://github.com/dotnet/efcore/issues/19129)
- [Json: add support for Sqlite provider](https://github.com/dotnet/efcore/issues/28816)
- [SQL Server: Support hierarchyid](https://github.com/dotnet/efcore/issues/365)
- [Configuration to opt out of occasionally problematic SaveChanges optimizations](https://github.com/dotnet/efcore/issues/29916)
- [Add convention types for triggers](https://github.com/dotnet/efcore/issues/28687)
- [Translate element access of a JSON array](https://github.com/dotnet/efcore/issues/28648)
- [Raw SQL queries for unmapped types](https://learn.microsoft.com/ef/core/what-is-new/ef-core-8.0/plan#sql-queries-for-unmapped-types)
- [Support the new BCL DateOnly and TimeOnly structs for SQL Server](https://github.com/dotnet/efcore/issues/24507)
- [Translate ElementAt(OrDefault)](https://github.com/dotnet/efcore/issues/17066)
- [Opt-out of lazy-loading for specific navigations](https://github.com/dotnet/efcore/issues/10787)
- [Lazy-loading for no-tracking queries](https://github.com/dotnet/efcore/issues/10042)
- [Reverse engineer Synapse and Dynamics 365 TDS](https://github.com/dotnet/efcore/issues/29121)
- [Set MaxLength on TPH discriminator property by convention](https://github.com/dotnet/efcore/issues/10691)
- [Translate ToString() on a string column](https://github.com/dotnet/efcore/issues/20839)
- [Generic overload of ConventionSetBuilder.Remove](https://github.com/dotnet/efcore/issues/29476)
- [Lookup tracked entities by primary key, alternate key, or foreign key](https://github.com/dotnet/efcore/issues/29685)
- [Allow UseSequence and HiLo on non-key properties](https://github.com/dotnet/efcore/issues/29758)
- [Pass query tracking behavior to materialization interceptor](https://github.com/dotnet/efcore/issues/29910)
- [Use case-insensitive string key comparisons on SQL Server](https://github.com/dotnet/efcore/issues/27526)
- [Allow value converters to change the DbType](https://github.com/dotnet/efcore/issues/24771)
- [Resolve application services in EF services](https://github.com/dotnet/efcore/issues/13540)
- [Numeric rowersion properties automatically convert to binary](https://github.com/dotnet/efcore/issues/12434)
- [Allow transfer of ownership of DbConnection from application to DbContext](https://github.com/dotnet/efcore/issues/24199)
- [Provide more information when 'No DbContext was found' error is generated](https://github.com/dotnet/efcore/issues/18715)

In this post we're going to take a more detailed look at [using complex types to represent value objects](https://github.com/dotnet/efcore/issues/9906).

## Complex types as value objects

Objects saved to the database can be split into three broad categories:

- Objects that are unstructured and hold a single value. For example, `int`, `Guid`, `string`, `IPAddress`. These are (somewhat loosely) called "primitive types".
- Objects that are structured to hold multiple values, and where the identity of the object is defined by a key value. For example, `Blog`, `Post`, `Customer`. These are called "entity types".
- Objects that are structured to hold multiple values, but the object has no key defining identity. For example, `Address`, `Coordinate`.

Prior to EF8, there was no good way to map the third type of object. [Owned types](https://learn.microsoft.com/ef/core/modeling/owned-entities) can be used, but since owned types are actually entity types, they have a key and semantics based on the value of that key, even when the key value is hidden.

EF8 now supports "Complex Types" to cover this third type of object. Complex types in EF Core are very similar to complex types in EF6, but there are some differencee. Complex type objects:

- Are not identified or tracked by key value.
- Must be defined as part of an entity type. (In other words, you cannot have a `DbSet` of a complex type.)
- Can be either .NET [value types](https://review.learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/value-types) or [reference types](https://review.learn.microsoft.com/dotnet/csharp/language-reference/keywords/reference-types). (EF6 only supports reference types.)
- Can share the same instance across multiple properties. (See below for more details. EF6 does not allow sharing.)

### Simple example

For example, consider an `Address` type:

```csharp
public class Address
{
    public required string Line1 { get; set; }
    public string? Line2 { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string PostCode { get; set; }
}
```

`Address` is then used in three places in a simple customer/orders model:

```csharp
public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Address Address { get; set; }
    public List<Order> Orders { get; } = new();
}

public class Order
{
    public int Id { get; set; }
    public required string Contents { get; set; }
    public required Address ShippingAddress { get; set; }
    public required Address BillingAddress { get; set; }
    public Customer Customer { get; set; } = null!;
}
```

Let's create and save a customer with their address:

```csharp
var customer = new Customer
{
    Name = "Willow",
    Address = new() { Line1 = "Barking Gate", City = "Walpole St Peter", Country = "UK", PostCode = "PE14 7AV" }
};

context.Add(customer);
await context.SaveChangesAsync();
```

This results in the following row being inserted into the database:

```sql
INSERT INTO [Customers] ([Name], [Address_City], [Address_Country], [Address_Line1], [Address_Line2], [Address_PostCode])
OUTPUT INSERTED.[Id]
VALUES (@p0, @p1, @p2, @p3, @p4, @p5);
```

Notice that the complex types do not get their own tables. Instead, they are saved inline to columns of the `Customers` table. This matches the table sharing behavior of owned types.

> **NOTE**
> We don't plan to allow complex types to be mapped to their own table. However, in a future release, we do plan to allow the complex type to be saved as a JSON document in a single column. Vote for [Issue #31252](https://github.com/dotnet/efcore/issues/31252) if this is important to you.

Now let's say we want to ship an order to a customer and use the customer's address as both the default billing an shipping address. The natural way to do this is to copy the `Address` object from the `Customer` into the `Order`. For example:

```csharp
customer.Orders.Add(
    new Order { Contents = "Tesco Tasty Treats", BillingAddress = customer.Address, ShippingAddress = customer.Address, });

await context.SaveChangesAsync();
```

With complex types, this works as expected, and the address is inserted into the `Orders` table:

```sql
INSERT INTO [Orders] ([Contents], [CustomerId],
    [BillingAddress_City], [BillingAddress_Country], [BillingAddress_Line1], [BillingAddress_Line2], [BillingAddress_PostCode],
    [ShippingAddress_City], [ShippingAddress_Country], [ShippingAddress_Line1], [ShippingAddress_Line2], [ShippingAddress_PostCode])
OUTPUT INSERTED.[Id]
VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11);
```

So far you might be saying, "but I could do this with owned types!" However, the "entity type" semantics of owned types quickly get in th way. For example, running the code above with owned types results in a slew of warnings and then an error:

```text
warn: 8/20/2023 12:48:01.678 CoreEventId.DuplicateDependentEntityTypeInstanceWarning[10001] (Microsoft.EntityFrameworkCore.Update) 
      The same entity is being tracked as different entity types 'Order.BillingAddress#Address' and 'Customer.Address#Address' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
warn: 8/20/2023 12:48:01.687 CoreEventId.DuplicateDependentEntityTypeInstanceWarning[10001] (Microsoft.EntityFrameworkCore.Update) 
      The same entity is being tracked as different entity types 'Order.ShippingAddress#Address' and 'Customer.Address#Address' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
warn: 8/20/2023 12:48:01.687 CoreEventId.DuplicateDependentEntityTypeInstanceWarning[10001] (Microsoft.EntityFrameworkCore.Update)
      The same entity is being tracked as different entity types 'Order.ShippingAddress#Address' and 'Order.BillingAddress#Address' with defining navigations. If a property value changes, it will result in two store changes, which might not be the desired outcome.
fail: 8/20/2023 12:48:01.709 CoreEventId.SaveChangesFailed[10000] (Microsoft.EntityFrameworkCore.Update) 
      An exception occurred in the database while saving changes for context type 'NewInEfCore8.ComplexTypesSample+CustomerContext'.
      System.InvalidOperationException: Cannot save instance of 'Order.ShippingAddress#Address' because it is an owned entity without any reference to its owner. Owned entities can only be saved as part of an aggregate also including the owner entity.
         at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry.PrepareToSave()
         ...
```

This is because a single instance of the `Address` entity type (with the same hidden key value) is being used for three _different_ entity instances. On the other hand, sharing the same instance between complex properties is allowed, and so the code works as expected when using complex types.

### Configuration of complex types

Complex types must be configured in the model using either [mapping attributes](https://learn.microsoft.com/ef/core/modeling/#use-data-annotations-to-configure-a-model) or by calling [`ComplexProperty` API in `OnModelCreating`](https://learn.microsoft.com/ef/core/modeling/#use-fluent-api-to-configure-a-model). Complex types are not discovered by convention.

For example, the `Address` type can be configured using the [`ComplexTypeAttribute`](https://review.learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.schema.complextypeattribute):

```csharp
[ComplexType]
public class Address
{
    public required string Line1 { get; set; }
    public string? Line2 { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required string PostCode { get; set; }
}
```

Or in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Customer>()
        .ComplexProperty(e => e.Address);

    modelBuilder.Entity<Order>(b =>
    {
        b.ComplexProperty(e => e.BillingAddress);
        b.ComplexProperty(e => e.ShippingAddress);
    });
}
```

### Mutability

In the example above, we ended up with the same `Address` instance used in three places. This is allowed and doesn't cause any issues for EF Core when using complex types. However, sharing instances of the same reference type means that if a property value on the instance is modified, then that change will be reflected in all three usages. For example, following on from above, let's change `Line1` of the customer address and save the changes:

```csharp
customer.Address.Line1 = "Peacock Lodge";
await context.SaveChangesAsync();
```

This results in the following update to the database when using SQL Server:

```sql
UPDATE [Customers] SET [Address_Line1] = @p0
OUTPUT 1
WHERE [Id] = @p1;
UPDATE [Orders] SET [BillingAddress_Line1] = @p2, [ShippingAddress_Line1] = @p3
OUTPUT 1
WHERE [Id] = @p4;
```

Notice that all three `Line1` columns have changed, since they are all sharing the same instance. This is usually not what we want.

> **TIP**
> If order addresses should change automatically when the customer address changes, then consider mapping the address as an entity type. `Order` and `Customer` can then safely reference the same address instance (which is now identified by a key) via a navigation property.

A good way to deal with issues like this it to make the type immutable. Indeed, this immutability often natural when a type is a good candidate for being a complex type. For example, it usually makes sense to supply a complex new `Address` object rather than to just mutate, say, the country while leaving the rest the same. C# records are a great fit for this!

### Immutable record

C# 9 introduced [record types](https://review.learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record?branch=main), which makes creating and using immutable objects easier. For example, the `Address` object can be made a record type:

```csharp
public record Address(string Line1, string? Line2, string City, string Country, string PostCode);
```

It now becomes very easy to update the immutable recored with just one property value changed. For example:

```csharp
customer.Address = customer.Address with { Line1 = "Peacock Lodge" };

await context.SaveChangesAsync();
```

### Immutable struct record

C# 10 introduced `struct record` types, which makes it easy to create and work with immutable struct records like it is with immutable class records. For example, we can define `Address` as an immutable struct record:

```csharp
public readonly record struct Address(string Line1, string? Line2, string City, string Country, string PostCode);
```

The code for changing the address now looks the same as when using an immutable class record:

```csharp
customer.Address = customer.Address with { Line1 = "Peacock Lodge" };

await context.SaveChangesAsync();
```

### Nested complex types

A complex type can contain properties of other complex types. For example, let's use our `Address` complex type from above together with a `PhoneNumber` complex type, and nest them both inside another complex type:

```csharp
public record Address(string Line1, string? Line2, string City, string Country, string PostCode);

public record PhoneNumber(int CountryCode, long Number);

public record Contact
{
    public required Address Address { get; init; }
    public required PhoneNumber HomePhone { get; init; }
    public required PhoneNumber WorkPhone { get; init; }
    public required PhoneNumber MobilePhone { get; init; }
}
```

We're using immutable records here, since these are a good match for the semantics of our complex types, but nesting of complex types can be done with any flavor of .NET type.

> **NOTE**
> We're not using a primary constructor for the `Contact` type because EF Core does not yet support constructor injection of complex type values. Vote for [Issue #31621](https://github.com/dotnet/efcore/issues/31621) if this is important to you.

We will add `Contact` as a property of the `Customer`:

```csharp
public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Contact Contact { get; set; }
    public List<Order> Orders { get; } = new();
}
```

And `PhoneNumber` as properties of the `Order`:

```csharp
public class Order
{
    public int Id { get; set; }
    public required string Contents { get; set; }
    public required PhoneNumber ContactPhone { get; set; }
    public required Address ShippingAddress { get; set; }
    public required Address BillingAddress { get; set; }
    public Customer Customer { get; set; } = null!;
}
```

Configuration of nested complex types can again be achieved using [`ComplexTypeAttribute`](https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations.schema.complextypeattribute):

```csharp
[ComplexType]
public record Address(string Line1, string? Line2, string City, string Country, string PostCode);

[ComplexType]
public record PhoneNumber(int CountryCode, long Number);

[ComplexType]
public record Contact
{
    public required Address Address { get; init; }
    public required PhoneNumber HomePhone { get; init; }
    public required PhoneNumber WorkPhone { get; init; }
    public PhoneNumrequired ber MobilePhone { get; init; }
}
```

Or in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Customer>(
        b =>
        {
            b.ComplexProperty(
                e => e.Contact,
                b =>
                {
                    b.ComplexProperty(e => e.Address);
                    b.ComplexProperty(e => e.HomePhone);
                    b.ComplexProperty(e => e.WorkPhone);
                    b.ComplexProperty(e => e.MobilePhone);
                });
        });

    modelBuilder.Entity<Order>(
        b =>
        {
            b.ComplexProperty(e => e.ContactPhone);
            b.ComplexProperty(e => e.BillingAddress);
            b.ComplexProperty(e => e.ShippingAddress);
        });
}
```

### Queries

Properties of complex types on entity types are treated like any other non-navigation property of the entity type. This means that they are always loaded when the entity type is loaded. This is also true of any nested complex type properties. For example, querying for a customer:

```csharp
var customer = await context.Customers.FirstAsync(e => e.Id == customerId);
```

Is translated to the following SQL when using SQL Server:

```sql
SELECT TOP(1) [c].[Id], [c].[Name], [c].[Contact_Address_City], [c].[Contact_Address_Country],
    [c].[Contact_Address_Line1], [c].[Contact_Address_Line2], [c].[Contact_Address_PostCode],
    [c].[Contact_HomePhone_CountryCode], [c].[Contact_HomePhone_Number], [c].[Contact_MobilePhone_CountryCode],
    [c].[Contact_MobilePhone_Number], [c].[Contact_WorkPhone_CountryCode], [c].[Contact_WorkPhone_Number]
FROM [Customers] AS [c]
WHERE [c].[Id] = @__customerId_0
```

Notice two things from this SQL:

- Everything is returned to populate the customer _and_ all the nested `Contact`, `Address`, and `PhoneNumber` complex types.
- All the complex type values are stored as columns in the table for the entity type. Complex types are never mapped to separate tables.

#### Projections

Complex types can be projected from a query. For example, selecting just the shipping address from an order:

```csharp
var shippingAddress = await context.Orders
    .Where(e => e.Id == orderId)
    .Select(e => e.ShippingAddress)
    .SingleAsync();
```

This translates to the following when using SQL Server:

```sql
SELECT TOP(2) [o].[ShippingAddress_City], [o].[ShippingAddress_Country], [o].[ShippingAddress_Line1],
    [o].[ShippingAddress_Line2], [o].[ShippingAddress_PostCode]
FROM [Orders] AS [o]
WHERE [o].[Id] = @__orderId_0
```

Note that projections of complex types cannot be tracked, since complex type objects have no identity to use for tracking.

### Use in predicates

Members of complex types can be used in predicates. For example, finding all the orders going to a certain city:

```csharp
var city = "Walpole St Peter";
var walpoleOrders = await context.Orders.Where(e => e.ShippingAddress.City == city).ToListAsync();
```

Which translates to the following SQL on SQL Server:

```sql
SELECT [o].[Id], [o].[Contents], [o].[CustomerId], [o].[BillingAddress_City], [o].[BillingAddress_Country],
    [o].[BillingAddress_Line1], [o].[BillingAddress_Line2], [o].[BillingAddress_PostCode],
    [o].[ContactPhone_CountryCode], [o].[ContactPhone_Number], [o].[ShippingAddress_City],
    [o].[ShippingAddress_Country], [o].[ShippingAddress_Line1], [o].[ShippingAddress_Line2],
    [o].[ShippingAddress_PostCode]
FROM [Orders] AS [o]
WHERE [o].[ShippingAddress_City] = @__city_0
```

A full complex type instance can also be used in predicates. For example, finding all customers with a given phone number:

```csharp
var phoneNumber = new PhoneNumber(44, 7777555777);
var customersWithNumber = await context.Customers
    .Where(
        e => e.Contact.MobilePhone == phoneNumber
             || e.Contact.WorkPhone == phoneNumber
             || e.Contact.HomePhone == phoneNumber)
    .ToListAsync();
```

This translates to the following SQL when using SQL Server:

```sql
SELECT [c].[Id], [c].[Name], [c].[Contact_Address_City], [c].[Contact_Address_Country], [c].[Contact_Address_Line1],
     [c].[Contact_Address_Line2], [c].[Contact_Address_PostCode], [c].[Contact_HomePhone_CountryCode],
     [c].[Contact_HomePhone_Number], [c].[Contact_MobilePhone_CountryCode], [c].[Contact_MobilePhone_Number],
     [c].[Contact_WorkPhone_CountryCode], [c].[Contact_WorkPhone_Number]
FROM [Customers] AS [c]
WHERE ([c].[Contact_MobilePhone_CountryCode] = @__entity_equality_phoneNumber_0_CountryCode
    AND [c].[Contact_MobilePhone_Number] = @__entity_equality_phoneNumber_0_Number)
OR ([c].[Contact_WorkPhone_CountryCode] = @__entity_equality_phoneNumber_0_CountryCode
    AND [c].[Contact_WorkPhone_Number] = @__entity_equality_phoneNumber_0_Number)
OR ([c].[Contact_HomePhone_CountryCode] = @__entity_equality_phoneNumber_0_CountryCode
    AND [c].[Contact_HomePhone_Number] = @__entity_equality_phoneNumber_0_Number)
```

Notice that equality is performed by expanding out each member of the complex type. This aligns with complex types having no key for identity and hence a complex type instance is equal to another complex type instance if and only if all their members are equal. Notice that this also aligns with equality for C# records.

### Manipulation of complex type values

EF8 provides access to tracking information such as the current and original values of complex types and whether or not a property value has been modified. The API complex types is an extension of [the change tracking API already used for entity types](https://learn.microsoft.com/ef/core/change-tracking/entity-entries).

The `ComplexProperty` methods of [EntityEntry](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.entityentry) return a entry for an entire complex object. For example, to get the current value of the `Order.BillingAddress`:

```csharp
var billingAddress = context.Entry(order)
    .ComplexProperty(e => e.BillingAddress)
    .CurrentValue;
```

A call to `Property` can be added to access a property of the complex type. For example to get the current value of just the billing post code:

```csharp
var postCode = context.Entry(order)
    .ComplexProperty(e => e.BillingAddress)
    .Property(e => e.PostCode)
    .CurrentValue;
```

Nested complex types are accessed using nested calls to `ComplexProperty`. For example, to get the city from the nested `Address` of the `Contact` on a `Customer`:

```csharp
var currentCity = context.Entry(customer)
    .ComplexProperty(e => e.Contact)
    .ComplexProperty(e => e.Address)
    .Property(e => e.City)
    .CurrentValue;
```

Other methods are available for reading and changing state. For example, [`PropertyEntry.IsModified`](https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.changetracking.propertyentry.ismodified#microsoft-entityframeworkcore-changetracking-propertyentry-ismodified) can be used to set a property of a complex type as modified:

```csharp
context.Entry(customer)
    .ComplexProperty(e => e.Contact)
    .ComplexProperty(e => e.Address)
    .Property(e => e.PostCode)
    .IsModified = true;
```

### Current limitations

Complex types represent a significant investment across the EF stack. We were not able to make everything work in this release, but we plan to close some of the gaps in a future release. Make sure to vote (👍) on the appropriate GitHub issues if fixing any of these limitations is important to you.

Complex type limitations in EF8 include:

- Support collections of complex types. ([Issue #31237](https://github.com/dotnet/efcore/issues/31237))
- Allow complex type properties to be null. ([Issue #31376](https://github.com/dotnet/efcore/issues/31376))
- Map complex type properties to JSON columns. ([Issue #31252](https://github.com/dotnet/efcore/issues/31252))
- Constructor injection for complex types. ([Issue #31621](https://github.com/dotnet/efcore/issues/31621))
- Add seed data support for complex types. ([Issue #31254](https://github.com/dotnet/efcore/issues/31254))
- Map complex type properties for the Cosmos provider. ([Issue #31253](https://github.com/dotnet/efcore/issues/31253))
- Implement complex types for the in-memory database. ([Issue #31464](https://github.com/dotnet/efcore/issues/31464))

## How to get EF8 RC 1

EF8 is distributed exclusively as a set of NuGet packages. For example, to add the SQL Server provider to your project, you can use the following command using the dotnet tool:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0-rc.1.23453.1
```

## Installing the EF8 Command Line Interface (CLI)

The `dotnet-ef` tool must be installed before executing EF8 Core migration or scaffolding commands.

To install the tool globally, use:

```bash
dotnet tool install --global dotnet-ef --version 8.0.0-rc.1.23453.1
```

If you already have the tool installed, you can upgrade it with the following command:

```bash
dotnet tool update --global dotnet-ef --version 8.0.0-rc.1.23453.1
```

## The .NET Data Community Standup

The .NET data access team is now live streaming every other Wednesday at 10am Pacific Time, 1pm Eastern Time, or 18:00 UTC. Join the stream learn and ask questions about many .NET Data related topics.

- [Watch our YouTube playlist](https://aka.ms/efstandups) of previous shows
- [Visit the .NET Community Standup](https://live.dot.net) page to preview upcoming shows
- [Submit your ideas](https://github.com/dotnet/efcore/issues/22700) for a guest, product, demo, or other content to cover

## Documentation and Feedback

The starting point for all EF Core documentation is [docs.microsoft.com/ef/](https://docs.microsoft.com/ef/). Please file issues found and any other feedback on the [dotnet/efcore GitHub repo](https://github.com/dotnet/efcore).

## Helpful Links

The following links are provided for easy reference and access.

- EF Core Community Standup Playlist: [aka.ms/efstandups](https://aka.ms/efstandups)
- Main documentation: [aka.ms/efdocs](https://aka.ms/efdocs)
- What's New in EF Core 8: [aka.ms/ef8-new](https://aka.ms/ef8-new)
- What's New in EF Core 7: [aka.ms/ef7-new](https://aka.ms/ef7-new)
- Issues and feature requests for EF Core: [github.com/dotnet/efcore/issues](https://github.com/dotnet/efcore/issues)
- Entity Framework Roadmap: [aka.ms/efroadmap](https://aka.ms/efroadmap)
- Bi-weekly updates: [aka.ms/ef-news](https://aka.ms/ef-news)
