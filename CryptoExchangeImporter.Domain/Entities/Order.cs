using System;
using System.Collections.Generic;
using System.Text;

public class Order
{
    public int Id { get; set; }
    // Natural key from JSON (unique).
    public Guid OrderId { get; set; }     // For type safety, performance and matching JSON data type.
    // TODO: Check in PR: Time and Type are SQL reserved words.
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }     // For type safety, performance, Clean Architecture.
    public OrderKind Kind { get; set; }     // For type safety, performance, Clean Architecture.
    public decimal Amount { get; set; }
    public decimal Price { get; set; }

    // FK
    public int OrderBookEntryId { get; set; }
    public OrderBookEntry OrderBookEntry { get; set; }
}
