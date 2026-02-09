using System;
using System.Collections.Generic;
using System.Text;

public class Order
{
    public int Id { get; set; }
    // Unique and given.
    public Guid OrderId { get; set; }
    public DateTime Time { get; set; }
    public OrderType Type { get; set; }
    public OrderKind Kind { get; set; }
    // TODO: Add precision later.
    public decimal Amount { get; set; }
    public decimal Price { get; set; }

    // FK
    public int OrderBookEntryId { get; set; }
    public OrderBookEntry OrderBookEntry { get; set; }
}
