namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBook
{
    private readonly List<OrderBookEntry> _entries = new();

    public OrderBook() { }

    public int Id { get; private set; }
    public int ExchangeId { get; private set; }

    // Only projections, not mapped in EF Core.
    public IReadOnlyCollection<OrderBookEntry> Bids => _entries.Where(e => e.IsBid)
                                                               .ToList()
                                                               .AsReadOnly();
    public IReadOnlyCollection<OrderBookEntry> Asks => _entries.Where(e => !e.IsBid)
                                                               .ToList()
                                                               .AsReadOnly();
    
    // Navigation Property
    public Exchange Exchange { get; private set; } = default!;

    public void AddBid(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _entries.Add(new OrderBookEntry(order, isBid: true));
    }

    public void AddAsk(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _entries.Add(new OrderBookEntry(order, isBid: false));
    }
}

