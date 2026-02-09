namespace CryptoExchangeImporter.Domain.Entities;

public class OrderBookEntry
{
    public int Id { get; set; }
    // TODO: Add ADR?
    public bool IsBid { get; set; } // true = Bid, false = Ask => Avoid junction table.

    // Foreign Key
    public int OrderBookId { get; set; }
    public OrderBook OrderBook { get; set; }

    // Navigation Property
    public Order Order { get; set; }
}
