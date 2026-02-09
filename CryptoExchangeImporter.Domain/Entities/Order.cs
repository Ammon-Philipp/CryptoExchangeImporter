using System;
using System.Collections.Generic;
using System.Text;

public class Order
{
    // TODO: Enforce immutability - FINANCE domain!
    // TODO: Choice: Record type? Init-Only Setter?
    // TODO: Also adapt in OrderConfiguration => EF Core must handle immutable entities correctly.
    // TODO: Also adapt JSON Deserialization.
    // TODO: Also adapt domain logic => ctor with validation?
    // TODO: => Then also remember EF Core needs to work with ctor!
    // TODO: => Then also adapt OrderBookEntry.

    public int Id { get; set; }
    // Natural key from JSON (unique).
    public Guid OrderId { get; set; }     // For type safety, performance and matching JSON data type.
    // TODO: Check in PR: Time and Type are SQL reserved words.
    // TODO: Implement DateTimeOffset usage in service that handles import.
    public DateTimeOffset Time { get; set; }
    public OrderType Type { get; set; }     // For type safety, performance, Clean Architecture.
    public OrderKind Kind { get; set; }     // For type safety, performance, Clean Architecture.
    public decimal Amount { get; set; }
    public decimal Price { get; set; }

    // FK
    public int OrderBookEntryId { get; set; }
    public OrderBookEntry OrderBookEntry { get; set; }
}
