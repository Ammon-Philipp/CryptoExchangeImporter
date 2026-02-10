namespace CryptoExchangeImporter.Domain.Entities;

// TODO: Check in PR: Could be record as of being a Value Object. => Here class due to EF Core pragmatism (easier mapping / update / tracking).
public sealed class AvailableFunds
{
    private AvailableFunds() { } // EF

    public AvailableFunds(decimal crypto, decimal euro)
    {
        Crypto = crypto;
        Euro = euro;
    }

    public int Id { get; private set; }
    public decimal Crypto { get; private set; }
    public decimal Euro { get; private set; }

    // FK
    public int ExchangeId { get; private set; }
    public Exchange Exchange { get; private set; } = default!;
}
