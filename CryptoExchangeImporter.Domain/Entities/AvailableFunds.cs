namespace CryptoExchangeImporter.Domain.Entities;

public sealed class
    AvailableFunds // TODO: Check in PR: Record due to Value Object. => Here class due to EF Core pragmatism (easier mapping / update / tracking).
{
    public int Id { get; set; }
    public decimal Crypto { get; set; }
    public decimal Euro { get; set; }

    // FK
    public int ExchangeId { get; set; }
    public Exchange Exchange { get; set; } = default!;
}
