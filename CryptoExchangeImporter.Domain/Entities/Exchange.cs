namespace CryptoExchangeImporter.Domain.Entities;

public sealed class Exchange
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public AvailableFunds AvailableFunds { get; set; } = default!;
    public OrderBook OrderBook { get; set; } = default!;
}
