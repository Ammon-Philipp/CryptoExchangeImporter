namespace CryptoExchangeImporter.Domain.Entities;

public sealed class Order
{
    // EF Core.
    private Order() { }

    public Order(Guid orderId,
                 DateTimeOffset time,
                 OrderType type,
                 OrderKind kind,
                 decimal amount,
                 decimal price
    )
    {
        if (orderId == Guid.Empty) { throw new ArgumentException("OrderId must not be empty.", nameof(orderId)); }

        if (time > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("Order time cannot be in the future",
                                        nameof(time)
            ); // TODO: Check in PR if DateTimeOffset.UtcNow.AddMinutes(5)) is recommended to handle server time sync issues.
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive."); // TODO: Check in PR.
        }

        if (price <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be positive."); // TODO: Check in PR.
        }

        OrderId = orderId;
        Time    = time.ToUniversalTime(); // Ensure UTC.
        Type    = type;
        Kind    = kind;
        Amount  = amount;
        Price   = price;
    }

    public int Id { get; private set; }

    // Natural key from JSON (unique).
    public Guid OrderId { get; private set; } // Data type for type safety, performance and matching JSON data type.

    // TODO: Check in PR: Time and Type are SQL reserved words.
    // TODO: Implement DateTimeOffset usage in import service.
    public DateTimeOffset Time { get; private set; }
    public OrderType Type { get; private set; } // Data type for type safety, performance, Clean Architecture.
    public OrderKind Kind { get; private set; } // Data type for type safety, performance, Clean Architecture.
    public decimal Amount { get; private set; }
    public decimal Price { get; private set; }

    // FK
    public int OrderBookEntryId { get; private set; }

    // Navigation Property
    public OrderBookEntry OrderBookEntry { get; private set; } = default!;
}
