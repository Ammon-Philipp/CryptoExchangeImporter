namespace CryptoExchangeImporter.Domain.Entities;

public sealed class Exchange
{
    private Exchange() { }

    internal Exchange(string exchangeId, DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(exchangeId))
        {
            throw new ArgumentException("ExchangeId must not be empty", nameof(exchangeId));
        }

        createdAt = createdAt.ToUniversalTime().ToOffset(TimeSpan.Zero);

        if (createdAt > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("Exchange create time cannot be in the future", nameof(createdAt));
        }

        ExchangeId = exchangeId;
        CreatedAt  = createdAt;
    }
    public int Id { get; private set; }

    // Natural key from JSON (unique).
    public string ExchangeId { get; private set; } = default!;
    public DateTimeOffset CreatedAt { get; private set; }

    // Navigation Properties
    public AvailableFunds AvailableFunds { get; private set; } = default!;
    public OrderBook OrderBook { get; private set; } = default!;
}
