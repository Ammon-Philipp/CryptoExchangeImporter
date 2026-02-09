namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBookEntry
{
    public int Id { get; private set; }
    // TODO: Add ADR? => Avoid separate tables and Unions for EF Core queries. Use enum instead for more than two collections.
    // TODO: Check decision in PR in order to prevent inconsistent state: How to make sure that only Bid Order gets IsBid = true?
    // TODO: Let domain entity OrderBook enforce consistency?
    public bool IsBid { get; private set; } // true = Bid, false = Ask.

    // FK
    public int OrderBookId { get; private set; }
    public OrderBook OrderBook { get; private set; } = default!;

    public Order Order { get; private set; } = default!;

    private OrderBookEntry() { } // EF Core

    public OrderBookEntry(bool isBid, Order order)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        IsBid = isBid;
    }
}
