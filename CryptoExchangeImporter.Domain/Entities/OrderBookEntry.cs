namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBookEntry
{
    public int Id { get; private set; }
    // TODO: Add ADR? => Avoid separate tables and Unions for EF Core queries. Use enum instead for more than two collections.
    // TODO: Check in PR: Decision ok? How to prevent inconsistent state? => Make sure only Bid Order gets IsBid = true!
    // TODO: Let domain entity OrderBook enforce consistency?
    public bool IsBid { get; private set; } // true = Bid, false = Ask.

    // FK
    public int OrderBookId { get; private set; }

    // Navigation Properties
    public OrderBook OrderBook { get; private set; } = default!;

    public Order Order { get; private set; } = default!;

    private OrderBookEntry() { } // EF Core

    public OrderBookEntry(bool isBid, Order order)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        IsBid = isBid;
    }
}
