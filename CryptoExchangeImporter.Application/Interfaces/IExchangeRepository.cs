using CryptoExchangeImporter.Domain;
using CryptoExchangeImporter.Domain.Entities;

namespace CryptoExchangeImporter.Application.Interfaces;

public interface IExchangeRepository
{
    Task<bool> ExistsAsync(string exchangeId, CancellationToken cancel);

    Task<AddExchangeResult> AddAsync(Exchange exchange, CancellationToken cancel);

    Task<IReadOnlyList<Exchange>> GetAllAsync(CancellationToken cancel);

    Task<Exchange?> GetByIdAsync(string id, CancellationToken cancel);

    Task<HashSet<string>> GetExistingOrderIdsAsync(IEnumerable<string> orderIds, CancellationToken cancel);
}

public sealed record AddExchangeResult(
    int ImportedExchanges,
    int ImportedOrders,
    int SkippedDuplicateOrders
);
