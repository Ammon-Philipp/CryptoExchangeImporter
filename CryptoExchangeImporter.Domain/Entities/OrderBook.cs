namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBook
{
    private readonly List<OrderBookEntry> _bids = new();
    private readonly List<OrderBookEntry> _asks = new();

    public OrderBook() { }

    public int Id { get; private set; }
    public int ExchangeId { get; private set; }
    public IReadOnlyCollection<OrderBookEntry> Bids => _bids.AsReadOnly(); // TODO: Check if AsReadOnly() is good here.
    public IReadOnlyCollection<OrderBookEntry> Asks => _asks.AsReadOnly(); // TODO: Check if AsReadOnly() is good here.

    // Navigation Property
    public Exchange Exchange { get; private set; } = default!;

    public void AddBid(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _bids.Add(new OrderBookEntry(order, isBid: true));
    }

    public void AddAsk(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        _asks.Add(new OrderBookEntry(order, isBid: false));
    }
}

