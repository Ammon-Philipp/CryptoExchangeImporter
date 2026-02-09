using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoExchangeImporter.Domain.Entities;

public class OrderBookEntry
{
    public int Id { get; set; }
    public bool IsBid { get; set; } // true = Bid, false = Ask

    // Foreign Key
    public int OrderBookId { get; set; }
    public OrderBook OrderBook { get; set; }

    // Navigation Property
    public Order Order { get; set; }
}
