namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBookEntry
{
    public int Id { get; set; }
    // TODO: Add ADR? => Avoid separate tables and Unions for EF Core queries. Use enum instead for more than two collections.
    public bool IsBid { get; set; } // true = Bid, false = Ask.

    // Foreign Key
    public int OrderBookId { get; set; }
    public OrderBook OrderBook { get; set; }

    // Navigation Property
    public Order Order { get; set; }
}
