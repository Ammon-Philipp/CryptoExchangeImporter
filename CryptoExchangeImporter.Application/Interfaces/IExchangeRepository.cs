using CryptoExchangeImporter.Domain;
using CryptoExchangeImporter.Domain.Entities;

namespace CryptoExchangeImporter.Application.Interfaces;

public interface IExchangeRepository
{
    Task<bool> ExistsAsync(string exchangeId, CancellationToken cancel);

    Task<AddExchangeResult> AddAsync(Exchange exchange, CancellationToken cancel);

    Task<IReadOnlyList<Exchange>> GetAllAsync(CancellationToken cancel);

    Task<Exchange?> GetByIdAsync(string id, CancellationToken cancel);
}

public sealed class AddExchangeResult
{
    public int ImportedExchanges { get; init; }
    public int ImportedOrders { get; init; }
    public int SkippedDuplicateOrders { get; init; }
}
