namespace CryptoExchangeImporter.Domain.Entities;

public sealed class OrderBook
{
    public int Id { get; set; }

    // Navigation Properties
    public int ExchangeId { get; set; }
    public Exchange Exchange { get; set; }

    public ICollection<OrderBookEntry> Bids { get; set; } = new List<OrderBookEntry>();
    public ICollection<OrderBookEntry> Asks { get; set; } = new List<OrderBookEntry>();
}
