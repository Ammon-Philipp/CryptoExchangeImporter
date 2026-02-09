namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBookEntry
{
    private OrderBookEntry() { } // EF Core

    internal OrderBookEntry(Order order, bool isBid)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        IsBid = isBid;
    }

    public int Id { get; private set; }
    public bool IsBid { get; private set; } // true = Bid, false = Ask.

    // FK
    public int OrderBookId { get; private set; }

    // Navigation Properties
    public OrderBook OrderBook { get; private set; } = default!;
    public Order Order { get; private set; } = default!;
}
