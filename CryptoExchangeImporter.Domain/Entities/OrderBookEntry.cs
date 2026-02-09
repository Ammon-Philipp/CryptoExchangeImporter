namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBookEntry
{
    public int Id { get; set; }
    // TODO: Add ADR? => Avoid separate tables and Unions for EF Core queries. Use enum instead for more than two collections.
    // TODO: Check decision in PR in order to prevent inconsistent state: How to make sure that only Bid Order gets IsBid = true?
    // TODO: Let domain entity OrderBook enforce consistency?

    // Foreign Key
    public int OrderBookId { get; set; }
    public OrderBook OrderBook { get; set; }

    // Navigation Property
    public Order Order { get; set; }
}
