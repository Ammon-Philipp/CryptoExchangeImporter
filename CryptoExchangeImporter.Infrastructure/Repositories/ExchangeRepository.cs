using CryptoExchangeImporter.Application.Interfaces;
using CryptoExchangeImporter.Domain;
using CryptoExchangeImporter.Domain.Entities;
using CryptoExchangeImporter.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace CryptoExchangeImporter.Infrastructure.Repositories;

public sealed class ExchangeRepository : IExchangeRepository
{
    private readonly ExchangeDbContext _db;

    public ExchangeRepository(ExchangeDbContext db) => _db = db;

    public Task<bool> ExistsAsync(string exchangeId, CancellationToken cancel)
        => _db.Exchanges
              .AsNoTracking()
              .AnyAsync(x => x.ExchangeId == exchangeId, cancel);

    public async Task<AddExchangeResult> AddAsync(Exchange exchange, CancellationToken cancel)
    {
        _db.Exchanges.Add(exchange);
        await _db.SaveChangesAsync(cancel);

        return new AddExchangeResult(
            1,
            CountOrders(exchange),
            0
        );
    }

    public async Task<IReadOnlyList<Exchange>> GetAllAsync(CancellationToken cancel)
        => await _db.Exchanges
            .AsNoTracking()
            .Include(x => x.AvailableFunds)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancel);

    public Task<Exchange?> GetByIdAsync(string id, CancellationToken cancel)
        => _db.Exchanges
            .AsNoTracking()
            .Include(x => x.AvailableFunds)
            .Include(x => x.OrderBook)
                .ThenInclude(ob => ob.Bids)
                .ThenInclude(e => e.Order)
            .Include(x => x.OrderBook)
                .ThenInclude(ob => ob.Asks)
                .ThenInclude(e => e.Order)
            .FirstOrDefaultAsync(x => x.ExchangeId == id, cancel);

    private static int CountOrders(Exchange exchange)
    {
        return (exchange.OrderBook?.Bids?.Count ?? 0) + (exchange.OrderBook?.Asks?.Count ?? 0);
    }

    public async Task<HashSet<string>> GetExistingOrderIdsAsync(IEnumerable<string> orderIds, CancellationToken cancel)
    {
        var ids = orderIds.Where(x => !string.IsNullOrWhiteSpace(x))
                          .Select(x => x.Trim())
                          .Distinct()
                          .ToList();

        if (ids.Count == 0)
        {
            return new HashSet<string>();
        }

        return await _db.Orders
                        .AsNoTracking()
                        .Where(o => ids.Contains(o.OrderId))
                        .Select(o => o.OrderId)
                        .ToHashSetAsync(cancel);
    }

}
